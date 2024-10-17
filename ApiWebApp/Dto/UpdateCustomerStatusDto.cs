namespace ApiWebApp.Dto
{
    public class UpdateCustomerStatusDto
    {
        public bool IsActive { get; set; }
        public DateTime UpdatedAt = DateTime.UtcNow;
    }
}
