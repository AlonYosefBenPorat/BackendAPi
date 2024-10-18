namespace ApiWebApp.Dto
{
    public class ResetPasswordDto
    {
        public string Password { get; set; } = string.Empty;
        public DateTime LastPasswordUpdated { get; set; }= DateTime.UtcNow;
      
    }
}
