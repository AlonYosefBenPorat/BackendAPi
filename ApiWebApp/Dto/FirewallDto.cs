using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class FirewallDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        
        [Required]
        public required string Brand { get; set; }

        [Required] 
        public required string Model { get; set; }
        public string? Version { get; set; }

        public string? SerialNumber { get; set; }

        [Required]
        public required string IpAddress { get; set; }
        public string? MacAddress { get; set; }
        [Required]
        public required DateTime License { get; set; }
        [Required] 
        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } =DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
       
        
    }
}
