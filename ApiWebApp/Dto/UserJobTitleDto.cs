namespace ApiWebApp.Dto
{
    public class UserJobTitleDto
    {
        public string JobTitle { get; set; }
        public DateTime UpdatedAt = DateTime.UtcNow;
    }
}
