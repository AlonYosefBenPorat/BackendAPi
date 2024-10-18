using ApiWebApp.Model;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class UserDto
    {
        [Required, MinLength(2), MaxLength(40)]
        public required string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public required string LastName { get; set; } 
        
        public DateTime? DateOfBirth { get; set; }

        [Required, MinLength(2), MaxLength(50)]
        public required string JobTitle { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastPasswordUpdated { get; set; }

        
        //Fields Frome IdentityUser
      
        [Required, EmailAddress]
        public required string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required, DataType(DataType.Password)]
        public required string Password { get; set; }
        
        [Required]
        public required string Role { get; set; }




        public bool IsEnabled { get; set; }
        public string? ProfileAlt { get; set; }
        public string? ProfileSrc { get; set; }


        public UserDto()
        {
            IsEnabled = true;
        }
    }
}