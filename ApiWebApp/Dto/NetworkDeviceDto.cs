using ApiWebApp.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Model
{
    public class NetworkDeviceDto
    {
        
        public Guid Id { get; set; } 
        public Guid CustomerId { get; set; }

        [Required, MinLength(3), MaxLength(40)]
        public required string Type { get; set; }

        [Required]
        public required string IpAddress { get; set; }

        public string? Model { get; set; }
        public string? Brand { get; set; }

        [Required]
        public required string Vendor { get; set; } 
        public string? SerialNumber { get; set; }
        public string? Description { get; set; } 
        public DateTime? WarrantyExpiration { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 
       
        
      

    }
}
