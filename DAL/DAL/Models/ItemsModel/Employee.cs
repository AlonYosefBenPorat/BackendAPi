

using System.ComponentModel.DataAnnotations;

namespace DAL.Models.ItemsModel;

public class Employee
{
    [Key]
    public Guid EmployeeId { get; set; }

    [Required,MinLength(4), MaxLength(25)]
    public required string FullName { get; set; }

    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required,Phone]
    public required string Phone { get; set; }

    public bool IsActive { get; set; }
    public string? JobTitle { get; set; }

    [Required]
    public required Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }
}
