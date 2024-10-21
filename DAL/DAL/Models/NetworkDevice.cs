
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
namespace DAL.Models
{
    public class NetworkDevice
    {
        [Key]
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

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }



        public Customer? Customer { get; set; }
        public NetworkDevice()
        {
            Id = Guid.NewGuid();
        }
    }
}