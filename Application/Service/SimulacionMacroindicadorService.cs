using Application.Dtos.RankingGeneral;
using Application.Dtos.Simulacion;
using Persistence.Context;
using Persistence.Entities;
using Persistence.Repositories;

namespace Application.Service
{
    public class SimulacionMacroindicadorService
    {
        private readonly SimulacionMacroindicadorRepository _repository;
        private readonly MacroindicadorRepository _macroRepo;

        public SimulacionMacroindicadorService(FutureVestContext context)
        {
            _repository = new SimulacionMacroindicadorRepository(context);
            _macroRepo = new MacroindicadorRepository(context);
        }

        public async Task<List<Macroindicador>> GetMacroindicadoresDisponiblesAsync(List<int> idsUsados)
        {
            var todos = await _macroRepo.GetAllAsync();
            return todos.Where(m => !idsUsados.Contains(m.Id)).ToList();
        }

        public async Task<List<SimulacionMacroindicadorDto>> GetAllAsync()
        {
            var simulados = await _repository.GetAllAsync();
            var macros = await _macroRepo.GetAllAsync();

            return simulados.Select(s => new SimulacionMacroindicadorDto
            {
                Id = s.Id,
                MacroindicadorId = s.MacroindicadorId,
                PesoSimulacion = s.PesoSimulacion,
                NombreMacroindicador = macros.FirstOrDefault(m => m.Id == s.MacroindicadorId)?.Name
            }).ToList();
        }

        public async Task<bool> CrearAsync(SimulacionMacroindicadorInputDto dto)
        {
            var sumaActual = await _repository.GetSumaPesosAsync();
            var nuevoPeso = dto.PesoSimulacion ?? 0;

            if (sumaActual + nuevoPeso > 1)
                return false;

            var yaExiste = await _repository.ExistsAsync(dto.MacroindicadorId ?? 0);
            if (yaExiste)
                return false;

            var entity = new SimulacionMacroindicador
            {
                MacroindicadorId = dto.MacroindicadorId!.Value,
                PesoSimulacion = dto.PesoSimulacion!.Value
            };

            await _repository.AddAsync(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(int id, SimulacionMacroindicadorInputDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            var sumaSinActual = await _repository.GetSumaPesosExcluyendoAsync(id);
            var nuevoPeso = dto.PesoSimulacion ?? 0;

            if (sumaSinActual + nuevoPeso > 1)
                return false;

            entity.PesoSimulacion = nuevoPeso;
            await _repository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            await _repository.DeleteAsync(entity.Id);
            return true;
        }

        public async Task<(List<RankingGeneralDto> Ranking, string? Mensaje)> GenerarSimulacionAsync(
          int anio,
          List<IndicadorPorPais> indicadores,
          List<Pais> paises)
        {
           // validar entrada
            if (indicadores == null || paises == null || !indicadores.Any() || !paises.Any())
                return (new(), "No hay datos suficientes para simular el ranking.");

            var simulados = (await _repository.GetAllAsync()).ToList();
            var macros = await _macroRepo.GetAllAsync();

            // validar simulados y macros
            if (!simulados.Any())
                return (new(), "No hay macroindicadores simulados registrados.");

            if (!macros.Any())
                return (new(), "No hay macroindicadores disponibles en el sistema.");

            var sumaPesos = simulados.Sum(s => s.PesoSimulacion);
            if (Math.Round(sumaPesos, 2) != 1.00m)
            {
                return (new(), "Se deben ajustar los pesos simulados hasta que la suma sea igual a 1.");
            }

            var paisesElegibles = new List<(int PaisId, string Nombre, string CodigoIso, List<(int MacroId, decimal Valor)>)>();

            foreach (var pais in paises)
            {
                var indicadoresPais = indicadores.Where(i => i.PaisId == pais.Id && i.Anio == anio).ToList();
                if (!indicadoresPais.Any()) continue;

                var tieneTodos = simulados.All(s => indicadoresPais.Any(i => i.MacroindicadorId == s.MacroindicadorId));
                if (!tieneTodos) continue;

                var valores = new List<(int MacroId, decimal Valor)>();
                foreach (var s in simulados)
                {
                    var indicador = indicadoresPais.FirstOrDefault(i => i.MacroindicadorId == s.MacroindicadorId);
                    if (indicador == null) continue;

                    valores.Add((MacroId: s.MacroindicadorId, Valor: indicador.Valor));
                }

                if (valores.Count == simulados.Count)
                {
                    paisesElegibles.Add((pais.Id, pais.Name, pais.CodigoIso, valores));
                }
            }

            if (paisesElegibles.Count < 2)
            {
                var unico = paisesElegibles.FirstOrDefault();
                var nombre = unico.Nombre ?? "desconocido";
                return (new(), $"Solo el país {nombre} cumple con los requisitos. Agrega más indicadores.");
            }

            var ranking = new List<RankingGeneralDto>();

            foreach (var pais in paisesElegibles)
            {
                decimal scoring = 0;

                foreach (var sim in simulados)
                {
                    var macro = macros.FirstOrDefault(m => m.Id == sim.MacroindicadorId);
                    if (macro == null) continue;

                    var valoresIndicador = paisesElegibles
                        .Select(p => p.Item4.FirstOrDefault(v => v.MacroId == sim.MacroindicadorId).Valor)
                        .ToList();

                    if (!valoresIndicador.Any()) continue;

                    var min = valoresIndicador.Min();
                    var max = valoresIndicador.Max();
                    var valor = pais.Item4.FirstOrDefault(v => v.MacroId == sim.MacroindicadorId).Valor;

                    decimal normalizado = (min == max) ? 0.5m :
                        macro.EsMejorMasAlto ? (valor - min) / (max - min) :
                                               (max - valor) / (max - min);

                    scoring += normalizado * sim.PesoSimulacion;
                }

                scoring = Math.Clamp(scoring, 0, 1);
                var tasaRetorno = 2 + (15 - 2) * scoring;

                ranking.Add(new RankingGeneralDto
                {
                    NombrePais = pais.Nombre,
                    CodigoIso = pais.CodigoIso,
                    Scoring = Math.Round(scoring, 4),
                    TasaRetorno = Math.Round(tasaRetorno, 2)
                });
            }

            var ordenados = ranking.OrderByDescending(r => r.Scoring).ToList();
            for (int i = 0; i < ordenados.Count; i++)
            {
                ordenados[i].Posicion = i + 1;
            }

            return (ordenados, null);
        }




    }
}