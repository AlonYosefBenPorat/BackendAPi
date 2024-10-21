using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Asset
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [Required]
        public required string Type { get; set; }
        public string? IpAddress { get; set; }

        public string? Url { get; set; }
        public DateTime? SupportExpiration { get; set; }
        public string? Notes { get; set; }

        public string? License { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }




        // Navigation property
        public Customer? Customer { get; set; }

        public Asset()
        {
            Id = Guid.NewGuid();

        }

    }
}
