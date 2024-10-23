using System.ComponentModel.DataAnnotations;

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
        public string? JobTitle { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
