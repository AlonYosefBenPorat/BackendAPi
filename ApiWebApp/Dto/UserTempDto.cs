using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ApiWebApp.Dto
{
    public class UserTempDto
    {
        [Required, MinLength(2), MaxLength(40)]
        public required string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public required string LastName { get; set; }
        public required DateTime DateOfBirth { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public required string JobTitle { get; set; } = "Employee";

        [Required, EmailAddress]
        public required string Email { get; set; }
        public string? ProfileAlt { get; set; } 
        public string? ProfileSrc { get; set; } 

        public bool IsImport { get; set; } = false;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
    }
}
