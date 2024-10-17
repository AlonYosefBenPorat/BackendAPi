namespace ApiWebApp.Dto
{
    public class UpdateUserStatusDto
    {
        public bool IsEnabled { get; set; }
        public DateTime UpdatedAt =  DateTime.UtcNow;
    }
}
