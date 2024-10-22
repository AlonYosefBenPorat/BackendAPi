using DAL.Data;
using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
public class CustomerPermissionController(WebAppContext context) : ControllerBase
{
    private readonly WebAppContext _context = context;

    [HttpPost]
    public async Task<IActionResult> SetPermission(UserPermission permission)
    {
        _context.UserPermissions.Add(permission);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePermission(int id, UserPermission permission)
    {
        var existingPermission = await _context.UserPermissions.FindAsync(id);
        if (existingPermission is null)
        {
            return NotFound();
        }

        existingPermission.CanRead = permission.CanRead;
        existingPermission.CanWrite = permission.CanWrite;
        existingPermission.CanDelete = permission.CanDelete;
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Other actions...
}
