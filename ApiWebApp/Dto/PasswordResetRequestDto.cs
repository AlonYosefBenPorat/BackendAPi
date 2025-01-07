

namespace DAL.Models;

public class PasswordResetRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string OldPassword { get; set; } = string.Empty;
}
