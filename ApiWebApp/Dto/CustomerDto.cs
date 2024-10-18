using ApiWebApp.DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Dto
{
    public class CustomerDto

    {
        public Guid Id { get; set; }


        [Required, MinLength(2), MaxLength(50)]
        public required string Name { get; set; }


        [Required, MinLength(2), MaxLength(25)]
        public required string Country { get; set; }


        [Required, MinLength(2), MaxLength(25)]
        public required string City { get; set; }

        [Required, MinLength(2), MaxLength(100)]
        public required string Address { get; set; }

        [Required, Phone]
        public required string Phone { get; set; }


        [Required, MinLength(2), MaxLength(25)]
        public required string ContactPerson { get; set; }
        
        [Required] 
        [RegularExpression
            (@"^(?!:\/\/)([a-zA-Z0-9-_]+\.)*[a-zA-Z0-9][a-zA-Z0-9-_]+\.[a-zA-Z]{2,11}?$", 
            ErrorMessage = "Invalid domain format")]

        public required string Domain { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } 

      
        public int BnNumber { get; set; }

        public bool IsActive { get; set; }
        public Image? Logo { get; set; }
    

   

  


        public CustomerDto()
        {
            IsActive = true;
            Id = Guid.NewGuid();
        }

    }
}
