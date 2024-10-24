using System.ComponentModel.DataAnnotations;
using DAL.Models.CrmModel;
using DAL.Models.utilitiesModel;

namespace DAL.Models.ItemsModel
{
    public class Customer
    {
        // Removed the unused 'firewalls' field

        [Key]
        public Guid Id { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public required string Name { get; set; }

        [Required, MinLength(2), MaxLength(25)]
        public string Country { get; set; } = string.Empty;

        [Required, MinLength(2), MaxLength(25)]
        public string City { get; set; } = string.Empty;

        [Required, MinLength(2), MaxLength(100)]
        public required string Address { get; set; }

        [Required, Phone]
        public required string Phone { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public required string ContactPerson { get; set; }

        [Required]
        [RegularExpression(
           @"^(?!:\/\/)([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+\.[a-zA-Z]{2,11}?$",
           ErrorMessage = "Invalid domain format")]
        public required string Domain { get; set; }

        public int BnNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public Image? Logo { get; set; }

        public ICollection<Server> Servers { get; set; } = new List<Server>();
        public ICollection<NetworkDevice> NetworkDevices { get; set; } = new List<NetworkDevice>();
        public ICollection<Firewall> Firewalls { get; set; } = new List<Firewall>();
        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public ICollection<Backup> Backups { get; set; } = new List<Backup>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

        public Customer()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;

        }
    }
}
