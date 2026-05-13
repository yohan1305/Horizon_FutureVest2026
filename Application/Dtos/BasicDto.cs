namespace Application.Dtos
{
    public class BasicDto<Tkey>
    {
        public required Tkey Id { get; set; }
        public required string Name { get; set; }
    }
}
