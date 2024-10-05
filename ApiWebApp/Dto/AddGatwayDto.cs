namespace ApiWebApp.Dto
{
    public class AddGatwayDto
    {
        public Guid Id { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Vendor { get; set; }

        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public string License { get; set; }
        public string SupportExpiration { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CustomerId { get; set; }
    }
}
