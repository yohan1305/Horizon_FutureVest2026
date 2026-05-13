
namespace Persistence.Entities
{
    public class SimulacionMacroindicador
    {
        public int Id { get; set; }

        public required int MacroindicadorId { get; set; }

        public required decimal PesoSimulacion { get; set; }

        // Navigation

        public Macroindicador? Macroindicador { get; set; }

    }
}
