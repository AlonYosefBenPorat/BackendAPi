using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerPermissionController : ControllerBase
{
    private readonly WebAppContext _context;

    public CustomerPermissionController(WebAppContext context)
    {
        _context = context;
    }

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
        if (existingPermission == null)
        {
            return NotFound();
        }

        existingPermission.CanRead = permission.CanRead;
        existingPermission.CanWrite = permission.CanWrite;
        await _context.SaveChangesAsync();
        return Ok();
    }

    // Other actions...
}
