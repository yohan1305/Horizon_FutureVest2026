using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class PaisEntityConfiguration : IEntityTypeConfiguration<Pais>
    {
        public void Configure(EntityTypeBuilder<Pais> builder)
        {
            #region Tabla y clave
            builder.ToTable("Paises");
            builder.HasKey(p => p.Id);
            #endregion

            #region Propiedades
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.CodigoIso).IsRequired().HasMaxLength(5);

            #endregion

            #region Relaciones
            builder.HasMany(p => p.Indicadores)
                   .WithOne(i => i.Pais)
                   .HasForeignKey(i => i.PaisId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
