

using DAL.Models.ItemsModel;
using DAL.Models.UsersModel;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.CrmModel
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; set; }

        [Required, MinLength(2), MaxLength(20)]
        public required string Title { get; set; }

        [Required, MinLength(2), MaxLength(20)]
        public required string Description { get; set; }

        
        
     

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public bool IsClosed { get; set; }

        [Required]
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public Guid ContactPersonId { get; set; }
        public Employee? ContactPerson { get; set; }





    }
}
