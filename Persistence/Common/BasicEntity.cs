namespace Persistence.Common
{
    public class BasicEntity<Tkey>
    {
        public required Tkey Id { get; set; }
        public required string Name { get; set; }
    }
}
