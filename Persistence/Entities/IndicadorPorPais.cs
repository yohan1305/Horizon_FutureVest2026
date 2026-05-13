namespace Persistence.Entities
{
    public class IndicadorPorPais 
    {
        public int Id { get; set; }

        public required int PaisId { get; set; }
        public required int MacroindicadorId { get; set; }
        public required decimal Valor { get; set; }
        public required int Anio { get; set; }

        // Navigation
        public Pais? Pais { get; set; }
        public Macroindicador? Macroindicador { get; set; }

    }
}
