using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class IndicadorPorPaisEntityConfiguration : IEntityTypeConfiguration<IndicadorPorPais>
    {
        public void Configure(EntityTypeBuilder<IndicadorPorPais> builder)
        {
            #region Tabla y clave
            builder.ToTable("IndicadoresPorPais");
            builder.HasKey(i => i.Id);
            #endregion

            #region Propiedades
            builder.Property(i => i.Valor).IsRequired().HasPrecision(10, 4); // Ej: 12345.6789
            builder.Property(i => i.Anio).IsRequired();
            #endregion

            #region Relaciones
            builder.HasOne(i => i.Pais)
                   .WithMany(p => p.Indicadores)
                   .HasForeignKey(i => i.PaisId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.Macroindicador)
                   .WithMany(m => m.Indicadores)
                   .HasForeignKey(i => i.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion

            #region Restricción compuesta
            builder.HasIndex(i => new { i.PaisId, i.MacroindicadorId, i.Anio })
                   .IsUnique()
                   .HasDatabaseName("IX_Indicador_UnicoPorPaisAnio");
            #endregion
        }
    }

}
