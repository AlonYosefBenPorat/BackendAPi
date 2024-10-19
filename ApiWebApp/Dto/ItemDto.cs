namespace ApiWebApp.Dto
{
    public class ItemDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public Guid? CustomerId { get; set; }
        public object? Details { get; set; }
    }
}
