using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Entities;


namespace Persistence.EntityConfigurations
{
    public class MacroindicadoresEntityConfigution : IEntityTypeConfiguration<Macroindicador>
    {
        public void Configure(EntityTypeBuilder<Macroindicador> builder)
        {
            #region Tabla y Clave

            builder.ToTable("Macroindicadores");
            builder.HasKey(m => m.Id);
            #endregion

            #region Propiedades
            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
            builder.Property(m => m.Peso).IsRequired().HasPrecision(4, 2); // Ej: 0.1234
            builder.Property(m => m.EsMejorMasAlto).IsRequired();
            #endregion

            #region Relaciones
            builder.HasMany(m => m.Indicadores)
                   .WithOne(i => i.Macroindicador)
                   .HasForeignKey(i => i.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(m => m.Simulaciones)
                   .WithOne(s => s.Macroindicador)
                   .HasForeignKey(s => s.MacroindicadorId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion







        }
    }
}
