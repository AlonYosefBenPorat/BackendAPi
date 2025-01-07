using System;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class TicketDto
    {
        public int TicketId { get; set; }

        [Required]
        public required string Title { get; set; }

        [Required]
        public required string Subject { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string Status { get; set; }

        public bool IsOpen { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public string? CustomerName { get; set; } // Made optional

        [Required]
        public Guid CustomerId { get; set; }

        public string? EmployeeName { get; set; } 
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhone { get; set; }

        [Required]
        public Guid ContactEmployeeId { get; set; }

        public string[] HashTag { get; set; } = Array.Empty<string>();
        // New properties
       public string? AssignedTo { get; set; }
    public string? AssignedToFirstName { get; set; }
    public string? AssignedToLastName { get; set; }
    public string? AssignedToEmail { get; set; }
        public List<UserActivityDto> UserActivities { get; set; } = new List<UserActivityDto>();
    }

    public class UserActivityDto
    {
        public int UserActivityId { get; set; }
        public int TicketId { get; set; }
        public string? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime StartTime { get; set; } 
        public DateTime EndTime { get; set; }
        public required string Description { get; set; }
    }
    public class ChangeEmployeeDto
    {
        [Required]
        public Guid ContactEmployeeId { get; set; }
    }


}