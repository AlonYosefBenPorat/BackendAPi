
using DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.DAL.Model
{
    public class Customer
    {
        internal object firewalls;

        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public string Domain { get; set; }
        public int BnNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public Logo Logo { get; set; }
       
        

    
        public ICollection<Server> Servers { get; set; } = new List<Server>();
        public ICollection<NetworkDevice> NetworkDevices { get; set; } = new List<NetworkDevice>();
        public ICollection<Firewall>Firewalls { get; set; } = new List<Firewall>();
        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
        public ICollection<Backup> Backups { get; set; } = new List<Backup>();
        

        public Customer()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }
    }
}
