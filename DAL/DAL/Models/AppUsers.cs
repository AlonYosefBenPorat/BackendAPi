
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.DAL.Model
{
    public class AppUsers: IdentityUser
    {
        [Required, MinLength(2), MaxLength(40)]
       public required string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(40)]
        public required string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }


        [Required, MinLength(2), MaxLength(50)]
        public required string JobTitle { get; set; }
        
        public bool IsEnabled { get; set; } 
        public Image? ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastLogon { get; set; }
        public DateTime? LastPasswordUpdated { get; set; }
       



    }
}
