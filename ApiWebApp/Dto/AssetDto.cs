
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class AssetDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [Required]
        public required string Type { get; set; }

        public string? IpAddress { get; set; }

        public string? Url { get; set; }
        public DateTime? SupportExpiration { get; set; }
        public string? Notes { get; set; }

        public string? License { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
       public DateTime? UpdatedAt { get; set; }

        
       
        

    }
}
