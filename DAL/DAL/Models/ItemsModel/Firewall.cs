using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace DAL.Models.ItemsModel
{
    public class Firewall
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }

        [Required]
        public required string Brand { get; set; }

        [Required]
        public required string Model { get; set; }

        public string? SerialNumber { get; set; }
        public string? Version { get; set; }

        [Required]
        public required string IpAddress { get; set; }



        public string? MacAddress { get; set; }

        [Required]
        public required DateTime License { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string? Notes { get; set; }

        public Customer? Customer { get; set; }

        public Firewall()
        {
            Id = Guid.NewGuid();

        }



    }
}
