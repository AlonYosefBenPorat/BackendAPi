namespace ApiWebApp.Dto
{
    public class UpdateCustomerDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ContactPerson { get; set; }
        public string Domain { get; set; }
        public int BnNumber { get; set; }
        public bool IsActive { get; set; }

        public DateTime UpdatedAt { get; set;} = DateTime.UtcNow;


    }
}
