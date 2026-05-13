using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;

namespace Persistence.EntityConfigurations
{
    public class ConfiguracionRetornoEntityConfiguration : IEntityTypeConfiguration<ConfiguracionRetorno>
    {
        public void Configure(EntityTypeBuilder<ConfiguracionRetorno> builder)
        {
            #region Tabla y clave
            builder.ToTable("ConfiguracionRetorno");
            builder.HasKey(c => c.Id);
            #endregion

            #region Propiedades
            builder.Property(c => c.TasaMinima).IsRequired().HasPrecision(5, 2); // Ej: 0.1234
            builder.Property(c => c.TasaMaxima).IsRequired().HasPrecision(5, 2);
            #endregion
        }
    }

}
