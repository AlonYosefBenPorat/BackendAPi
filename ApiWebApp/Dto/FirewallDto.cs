namespace ApiWebApp.Dto
{
    public class FirewallDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Version { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }
        public DateTime License { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
