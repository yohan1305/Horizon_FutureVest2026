using Persistence.Common;

namespace Persistence.Entities
{
    public class Pais : BasicEntity<int>
    {
        public required string CodigoIso { get; set; }

        //Navegacion
        public ICollection<IndicadorPorPais>? Indicadores{ get; set; }
    }
}
