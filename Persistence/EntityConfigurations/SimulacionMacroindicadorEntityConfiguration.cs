using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class SimulacionMacroindicadorEntityConfiguration : IEntityTypeConfiguration<SimulacionMacroindicador>
    {
        public void Configure(EntityTypeBuilder<SimulacionMacroindicador> builder)
        {
            #region Tabla y clave
            builder.ToTable("SimulacionMacroindicadores");
            builder.HasKey(s => s.Id);
            #endregion

            #region Propiedades
            builder.Property(s => s.PesoSimulacion).IsRequired().HasPrecision(5, 4); // Ej: 0.1234
            #endregion

            #region Relaciones
            builder.HasOne(s => s.Macroindicador)
                   .WithMany(m => m.Simulaciones)
                   .HasForeignKey(s => s.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Restricción única
            builder.HasIndex(s => s.MacroindicadorId)
                   .IsUnique()
                   .HasDatabaseName("IX_Simulacion_UnicaPorMacroindicador");
            #endregion
        }
    }

}
