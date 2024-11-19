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


    [HttpGet("user/{userId}/all")]
    public async Task<IActionResult> GetPermissionsByUserId(Guid userId)
    {
        var permissions = await _context.UserPermissions
            .Where(p => p.UserId == userId)
            .ToListAsync();

        // Return an empty list with a 200 OK status if no permissions are found
        return Ok(permissions);
    }

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
            .FirstOrDefaultAsync(p => p.UserId == permission.UserId && p.CustomerId == permission.CustomerId);

        if (existingPermission != null)
        {
            // Update the existing permission for the specific customer
            existingPermission.CanRead = permission.CanRead;
            existingPermission.CanWrite = permission.CanWrite;
            existingPermission.CanDelete = permission.CanDelete;
        }
        else
        {
            // Add the new permission for the specific customer
            _context.UserPermissions.Add(permission);
        }

        await _context.SaveChangesAsync();
        return Ok();
    }




    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeletePermissionsByUserId(Guid userId)
    {
        var permissions = await _context.UserPermissions
            .Where(p => p.UserId == userId)
            .ToListAsync();

        if (!permissions.Any())
        {
            return NotFound();
        }

        _context.UserPermissions.RemoveRange(permissions);
        await _context.SaveChangesAsync();
        return Ok();
    }
}