using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
    }

}
