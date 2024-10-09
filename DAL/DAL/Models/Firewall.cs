using ApiWebApp.DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Firewall
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Version { get; set; }
        public string Model { get; set; }

        public string SerialNumber { get; set; }
        public string IpAddress { get; set; }
        public string MacAddress { get; set; }

        public DateTime License { get; set; } 
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        public Customer Customer { get; set; }

        public Firewall()
        {
            Id = Guid.NewGuid();
           
        }
   
        

    }
}
