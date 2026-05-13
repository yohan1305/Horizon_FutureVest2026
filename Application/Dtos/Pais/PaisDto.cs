
namespace Application.Dtos.Pais
{
    public class PaisDto : BasicDto<int>
    {
        public required string CodigoIso { get; set; }
    }
}
