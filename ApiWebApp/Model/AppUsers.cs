using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Model
{
    public class AppUsers: IdentityUser
    {
       public string? FirstName { get; set; }
        public string? LastName { get; set; }

       
        
    }
}
