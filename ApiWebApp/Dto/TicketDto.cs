using System;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class TicketDto
    {
        public int TicketId { get; set; }

        [Required, MinLength(2), MaxLength(20)]
        public required string Title { get; set; }

        [Required, MinLength(2), MaxLength(20)]
        public required string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

     

        [Required]
        public Guid EmployeeId { get; set; }
    }
}
