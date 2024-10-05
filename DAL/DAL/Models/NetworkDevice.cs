
using System;
using System.ComponentModel.DataAnnotations;
namespace ApiWebApp.DAL.Model
{
    public class NetworkDevice
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string IpAddress { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string Vendor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public string Description { get; set; }
    
        // Navigation property
        public Customer Customer { get; set; }
        public NetworkDevice()
        {
            Id = Guid.NewGuid();
        }
    }
}