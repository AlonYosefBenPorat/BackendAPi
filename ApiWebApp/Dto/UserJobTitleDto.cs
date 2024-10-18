namespace ApiWebApp.Dto
{
    public class UserJobTitleDto
    {
        public string JobTitle { get; set; } = string.Empty;
        public DateTime UpdatedAt = DateTime.UtcNow;
    }
}
