using DAL.Models;
using Microsoft.AspNetCore.Identity;


namespace DAL.Repositories;

public class UserRepository(UserManager<AppUsers> userManager, RoleManager<IdentityRole> roleManager) : IUserRepository
{
    private readonly UserManager<AppUsers> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;

    public async Task<IEnumerable<AppUsers>> GetAllUsersAsync()
    {
        return await Task.Run(() => _userManager.Users.ToList());

    }

    public async Task<AppUsers> GetUserByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            throw new NotImplementedException($"{id} Of User Item not found.");
        return user;
    }

    public async Task<IdentityResult> CreateUserAsync(AppUsers user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> UpdateUserAsync(AppUsers user)
    {
        return await _userManager.UpdateAsync(user);
    }

    public async Task<IdentityResult> DeleteUserAsync(AppUsers user)
    {
        return await _userManager.DeleteAsync(user);
    }

    public async Task<IList<string>> GetUserRolesAsync(AppUsers user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(AppUsers user, string role)
    {
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> RemoveUserFromRolesAsync(AppUsers user, IEnumerable<string> roles)
    {
        return await _userManager.RemoveFromRolesAsync(user, roles);
    }

    public async Task<bool> RoleExistsAsync(string role)
    {
        return await _roleManager.RoleExistsAsync(role);
    }
}
