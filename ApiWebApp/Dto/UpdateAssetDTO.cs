namespace ApiWebApp.Dto
{
    public class UpdateAssetDTO
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Type { get; set; }
        public string IpAddress { get; set; }

        public string Url { get; set; }

        public string License { get; set; }

        public string SupportExpiration { get; set; }
        public string Notes { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
