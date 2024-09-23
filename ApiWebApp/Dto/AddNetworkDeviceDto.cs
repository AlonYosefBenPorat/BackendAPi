using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Model
{
    public class AddNetworkDeviceDto
    {
        
        public Guid Id { get; set; }
        public string? IpAddress { get; set; }
       
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }

        public string? Brand { get; set; }
        public string? Type { get; set; }
        public string? Vendor { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public DateTime WarrantyExpiration { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
