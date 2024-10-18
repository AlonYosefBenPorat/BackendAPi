namespace ApiWebApp.Dto
{
    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public DateTime LastPasswordUpdated { get; set; }= DateTime.UtcNow;
      
    }
}
