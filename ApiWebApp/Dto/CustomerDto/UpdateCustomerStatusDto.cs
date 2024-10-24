namespace ApiWebApp.Dto.CustomerDto
{
    public class UpdateCustomerStatusDto
    {
        public bool IsActive { get; set; }
        public DateTime UpdatedAt = DateTime.UtcNow;
    }
}
