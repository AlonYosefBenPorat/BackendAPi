

namespace ApiWebApp.Dto
{
    public class UserResetPasswordDto
    {
        public string? Email { get; set; }
        public string? Token { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
