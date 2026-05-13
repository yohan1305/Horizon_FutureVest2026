using Application.Dtos.RankingGeneral;
using Persistence.Context;
using Persistence.Repositories;

namespace Application.Service
{
    public class RankingGeneralService
    {
        private readonly IndicadorPorPaisRepository _indicadorRepository;
        private readonly MacroindicadorRepository _macroRepository;
        private readonly PaisRepository _paisRepository;
        private readonly ConfiguracionRetornoRepository _retornoRepository;

        public RankingGeneralService(FutureVestContext context)
        {
            _indicadorRepository = new IndicadorPorPaisRepository(context);
            _macroRepository = new MacroindicadorRepository(context);
            _paisRepository = new PaisRepository(context);
            _retornoRepository = new ConfiguracionRetornoRepository(context);
        }


        public async Task<(List<RankingGeneralDto> ranking, string? mensaje)> CalcularRankingAsync(int anio)
        {
            var indicadores = await _indicadorRepository.GetAllAsync();
            var macros = (await _macroRepository.GetAllAsync()).Where(m => m.Peso > 0).ToList();
            var paises = await _paisRepository.GetAllAsync();

            // Validar suma de pesos
            var sumaPesos = macros.Sum(m => m.Peso);
            if (Math.Round(sumaPesos, 2) != 1.00m)
            {
                return (new(), "Se deben ajustar los pesos de los macroindicadores registrado hasta que la suma de lo mismo sea igual a 1");
            }

            // Filtrar países elegibles
            var paisesElegibles = new List<(int PaisId, string Nombre, string CodigoIso, List<(int MacroId, decimal Valor)>)>();

            foreach (var pais in paises)
            {
                var indicadoresPais = indicadores
                    .Where(i => i.PaisId == pais.Id && i.Anio == anio)
                    .ToList();

                var tieneTodos = macros.All(m => indicadoresPais.Any(i => i.MacroindicadorId == m.Id));
                if (tieneTodos)
                {
                    var valores = macros.Select(m =>
                    {
                        var valor = indicadoresPais.First(i => i.MacroindicadorId == m.Id).Valor;
                        return (MacroId: m.Id, Valor: valor);
                    }).ToList();

                    paisesElegibles.Add((pais.Id, pais.Name, pais.CodigoIso, valores));
                }
            }

            if (paisesElegibles.Count < 2)
            {
                var unico = paisesElegibles.FirstOrDefault();
                var nombre = unico.Nombre ?? "desconocido";
                return (new(), $"No hay suficiente países para poder calcular el ranking y la tasa de retorno, el único país que cumple con los requisitos es {nombre}, debe agregar más indicadores a los demás países en el año seleccionado");
            }

            // Normalización por indicador
            var ranking = new List<RankingGeneralDto>();

            foreach (var pais in paisesElegibles)
            {
                decimal scoring = 0;

                foreach (var macro in macros)
                {
                    var valoresIndicador = paisesElegibles
                        .Select(p => p.Item4.First(v => v.MacroId == macro.Id).Valor)
                        .ToList();

                    var min = valoresIndicador.Min();
                    var max = valoresIndicador.Max();
                    var valor = pais.Item4.First(v => v.MacroId == macro.Id).Valor;

                    decimal normalizado;
                    if (min == max)
                    {
                        normalizado = 0.5m;
                    }
                    else if (macro.EsMejorMasAlto)
                    {
                        normalizado = (valor - min) / (max - min);
                    }
                    else
                    {
                        normalizado = (max - valor) / (max - min);
                    }

                    var subpuntaje = normalizado * macro.Peso;
                    scoring += subpuntaje;
                }

                scoring = Math.Clamp(scoring, 0, 1);

                // Obtener tasas de retorno
                var retornoConfig = (await _retornoRepository.GetAllAsync()).FirstOrDefault();
                var rmin = retornoConfig?.TasaMinima ?? 2;
                var rmax = retornoConfig?.TasaMaxima ?? 15;

                var tasaRetorno = rmin + (rmax - rmin) * scoring;

                ranking.Add(new RankingGeneralDto
                {
                    NombrePais = pais.Nombre,
                    CodigoIso = pais.CodigoIso,
                    Scoring = Math.Round(scoring, 4),
                    TasaRetorno = Math.Round(tasaRetorno, 2)
                });
            }

            // Ordenar y asignar posición
            var ordenados = ranking.OrderByDescending(r => r.Scoring).ToList();
            for (int i = 0; i < ordenados.Count; i++)
            {
                ordenados[i].Posicion = i + 1;
            }

            return (ordenados, null);
        }
    }
}