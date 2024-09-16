using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto;

public class UpdateUserDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Role { get; set; }
    public DateTime? DateOfBirth { get; set; }
    
    public required string JobTitle { get; set; }
    public required bool IsEnabled { get; set; }
    public string? ProfileImage { get; set; }
    public DateTime UpdatedAt { get; set; } 

}