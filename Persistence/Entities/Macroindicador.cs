using Persistence.Common;

namespace Persistence.Entities
{
    public class Macroindicador : BasicEntity<int>
    {
        public required decimal Peso { get; set; }
        public required bool EsMejorMasAlto { get; set; }

        //Navegation
        public ICollection<IndicadorPorPais>? Indicadores { get; set; }
        public ICollection<SimulacionMacroindicador>? Simulaciones { get; set; }

    }
}
