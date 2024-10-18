using ApiWebApp.DAL.Model;
using ApiWebApp.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.DTOs
{
    public class ServerDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [Required, MinLength(1), MaxLength(50) ]
        public required string Hostname { get; set; }
        
        [Required]
        public required string IpAddress { get; set; }
        public string? Model { get; set; } 
        public string? Brand { get; set; }

        [Required]
        public required string Type { get; set; }
        public string? SerialNumber { get; set; }
        
       
        public string? Vendor { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? OperatingSystem { get; set; }
        public string? Roles { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? WarrantyExpiration { get; set; }
        
       
    }
}
