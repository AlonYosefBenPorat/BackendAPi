using DAL.Models.ItemsModel;
using DAL.Models.UsersModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models.CrmModel
{
    public class Ticket
    {
        public int TicketId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Subject { get; set; }

        [Required]
        public required string Description { get; set; }
        public required string Status { get; set; }

        public bool IsOpen { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public required string CustomerName { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public required string EmployeeName { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhone { get; set; }
        public string? AssignedTo { get; set; }
        public AppUsers? AssignedUser { get; set; } 
        public List<UserActivity> UserActivities { get; set; } = new List<UserActivity>();

        [Required]
        public Guid ContactEmployeeId { get; set; }

        public string[] HashTag { get; set; } = Array.Empty<string>();

        public Customer? Customer { get; set; }

        public Employee? ContactPerson { get; set; }
    }


    public class UserActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserActivityId { get; set; }
        public int TicketId { get; set; }
        public Ticket? Ticket { get; set; }

        public string? UserId { get; set; } 
        public AppUsers? User { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        [Required]
        public required string Description { get; set; }
    }
}




