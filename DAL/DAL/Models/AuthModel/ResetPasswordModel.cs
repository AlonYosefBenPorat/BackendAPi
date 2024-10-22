namespace DAL.Models.AuthModel
{
    public class ResetPasswordModel
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
