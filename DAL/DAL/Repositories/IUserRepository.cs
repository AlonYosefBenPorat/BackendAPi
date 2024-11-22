using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Identity;


public interface IUserRepository
{
    Task<IEnumerable<AppUsers>> GetAllUsersAsync();
    Task<AppUsers> GetUserByIdAsync(string id);
    Task<IdentityResult> CreateUserAsync(AppUsers user, string password);
    Task<IdentityResult> UpdateUserAsync(AppUsers user);
    Task<IdentityResult> DeleteUserAsync(AppUsers user);
    Task<IList<string>> GetUserRolesAsync(AppUsers user);
    Task<IdentityResult> AddUserToRoleAsync(AppUsers user, string role);
    Task<IdentityResult> RemoveUserFromRolesAsync(AppUsers user, IEnumerable<string> roles);
    Task<bool> RoleExistsAsync(string role);
}
