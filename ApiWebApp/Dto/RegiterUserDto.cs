using ApiWebApp.Model;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class RegiterUserDto
    {
        [Required, MinLength(2), MaxLength(25)]
        public required string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(25)]
        public required string LastName { get; set; }

        [Required, DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required, DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public required string Role { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [Required]
        public required string JobTitle { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
    
        public bool IsEnabled { get; set; }
        public string? ProfileAlt { get; set; }
        public string? ProfileSrc { get; set; }


        public RegiterUserDto()
        {
            IsEnabled = true;
        }
    }
}