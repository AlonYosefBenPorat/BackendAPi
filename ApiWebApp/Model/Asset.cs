using System.ComponentModel.DataAnnotations;

namespace ApiWebApp.Model
{
    public class Asset
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Type { get; set; }
        public string IpAddress { get; set; }
       
        public string Url{ get; set; }
      
       public string License { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
       
        public string SupportExpiration { get; set; }
        public string Notes { get; set; }
        // Navigation property
        public Customer Customer { get; set; }

            public Asset()
        {
            Id = Guid.NewGuid();
        }

    }
}
