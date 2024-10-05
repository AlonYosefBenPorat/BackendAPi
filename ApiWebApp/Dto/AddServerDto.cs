using ApiWebApp.DAL.Model;
using ApiWebApp.Model;
using System;

namespace ApiWebApp.DTOs
{
    public class AddServerDto
    {
        public Guid Id { get; set; }
        public string? IpAddress { get; set; }
        public string? Hostname { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Brand { get; set; }
        public string? Type { get; set; }
        public string? Vendor { get; set; }
        public string? Ram { get; set; }
        public string? Storage { get; set; }
        public string? OperatingSystem { get; set; }
        public string? Roles { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? WarrantyExpiration { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}
