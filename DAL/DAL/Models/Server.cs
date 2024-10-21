using System.ComponentModel.DataAnnotations;

namespace DAL.Models;


public class Server
{
    [Key]
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }

    [Required, MinLength(1), MaxLength(50)]
    public string Hostname { get; set; } = string.Empty;

    [Required]
    public string IpAddress { get; set; } = "0.0.0.0";

    public string? Model { get; set; }
    public string? Brand { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;
    public string? SerialNumber { get; set; }
    public string? Vendor { get; set; }
    public string? Ram { get; set; }
    public string? Storage { get; set; }
    public string? OperatingSystem { get; set; }
    public DateTime? WarrantyExpiration { get; set; }
    [Required]
    public string? Roles { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Customer? Customer { get; set; }
    public Server()
    {
        Id = Guid.NewGuid();
    }
}