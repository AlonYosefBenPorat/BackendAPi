using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models.UsersModel
{
    public class AppUsersTemp
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public required string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public required string LastName { get; set; }
        [Required]
        public required DateTime DateOfBirth { get; set; }

      
        public string? JobTitle { get; set; }

        [Required,EmailAddress]
        public required string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        
    }
}
