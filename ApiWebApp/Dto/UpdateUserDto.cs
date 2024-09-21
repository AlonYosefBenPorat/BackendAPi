using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto;

public class UpdateUserDto
{
    public  string? FirstName { get; set; }
    public  string? LastName { get; set; }
    public  string? PhoneNumber { get; set; }
    
    
    public  string? Role { get; set; }
    
    
    public string? JobTitle { get; set; }

    public  bool IsEnabled { get; set; }
    public string? ProfileAlt { get; set; }
    public string? ProfileSrc { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

}