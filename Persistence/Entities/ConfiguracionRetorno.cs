
namespace Persistence.Entities
{
    public class ConfiguracionRetorno
    {
        public int Id { get; set; }

        public required decimal TasaMinima { get; set; }

        public required decimal TasaMaxima { get; set; }

    }
}
