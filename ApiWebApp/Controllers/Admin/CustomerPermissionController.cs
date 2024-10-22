using DAL.Data;
using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiWebApp.Controllers.Admin;

[ApiController]
[Route("api/[controller]")]
//[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
public class CustomerPermissionController(WebAppContext context) : ControllerBase
{
    private readonly WebAppContext _context = context;

    [HttpGet]
    public async Task<IActionResult> GetPermissions()
    {
        var permissions = await _context.UserPermissions.ToListAsync();
        return Ok(permissions);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetPermissionByUserId(Guid userId)
    {
        var permission = await _context.UserPermissions
            .FirstOrDefaultAsync(p => p.UserId == userId);
        if (permission is null)
        {
            return NotFound();
        }

        return Ok(permission);
    }

    [HttpPost]
    public async Task<IActionResult> SetPermission(UserPermission permission)
    {
        var existingPermission = await _context.UserPermissions
            .FirstOrDefaultAsync(p => p.UserId == permission.UserId);

        if (existingPermission != null)
        {
            // Update the existing permission
            existingPermission.CanRead = permission.CanRead;
            existingPermission.CanWrite = permission.CanWrite;
            existingPermission.CanDelete = permission.CanDelete;
        }
        else
        {
            // Add the new permission
            _context.UserPermissions.Add(permission);
        }

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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePermission(int id)
    {
        var permission = await _context.UserPermissions.FindAsync(id);
        if (permission is null)
        {
            return NotFound();
        }

        _context.UserPermissions.Remove(permission);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
