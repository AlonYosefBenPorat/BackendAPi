using ApiWebApp.Model;

namespace ApiWebApp.Dto
{
    public class AddAssetDto
    {
        public string Type { get; set; }
        public string IpAddress { get; set; }

        public string Url { get; set; }

        public string License { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       


        public string SupportExpiration { get; set; }
        public string Notes { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
