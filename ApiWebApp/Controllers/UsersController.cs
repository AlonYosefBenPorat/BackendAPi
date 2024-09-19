using ApiWebApp.Dto;
using ApiWebApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [EnableCors("AllowAll")]
    [HttpGet]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Reviewer")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var userList = new List<object>();

        foreach (var user in users)
        {
            var roles = await _userRepository.GetUserRolesAsync(user);
            userList.Add(new
            {
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.FirstName,
                user.LastName,
                user.DateOfBirth,
                user.JobTitle,
                user.IsEnabled,
                user.ProfileImage,
                user.CreatedAt,
                user.UpdatedAt,
                Roles = roles
            });
        }

        return Ok(userList);
    }

    [EnableCors("AllowAll")]
    [HttpGet("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Reviewer")]
    public async Task<IActionResult> GetUser(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user != null)
        {
            var roles = await _userRepository.GetUserRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.Email,
                user.PhoneNumber,
                user.FirstName,
                user.LastName,
                user.DateOfBirth,
                user.JobTitle,
                user.ProfileImage,
                user.IsEnabled,
                Roles = roles,
            });
        }
        return NotFound();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
    public async Task<IActionResult> Register([FromBody] RegiterUserDto registerUserDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registerUserDto.Password != registerUserDto.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "The password and confirmation password do not match.");
                return BadRequest(ModelState);
            }

            var user = new AppUsers
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                PhoneNumber = registerUserDto.PhoneNumber,
                DateOfBirth = registerUserDto.DateOfBirth,
                JobTitle = registerUserDto.JobTitle,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            var result = await _userRepository.CreateUserAsync(user, registerUserDto.Password);
            if (result.Succeeded)
            {
                if (!await _userRepository.RoleExistsAsync(registerUserDto.Role))
                {
                    return BadRequest($"Role '{registerUserDto.Role}' does not exist.");
                }

                await _userRepository.AddUserToRoleAsync(user, registerUserDto.Role);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
                {
                    user.Id,
                    user.Email,
                    user.PhoneNumber,
                    user.FirstName,
                    user.LastName,
                    user.DateOfBirth,
                    user.JobTitle,
                    user.UserName
                });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error. Please try again later.");
        }
    }

    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Reviewer")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            ModelState.AddModelError("UserNotFound", "User with the specified ID was not found.");
            return NotFound(ModelState);
        }

        if (updateUserDto.FirstName != null)
        {
            user.FirstName = updateUserDto.FirstName;
        }

        if (updateUserDto.LastName != null)
        {
            user.LastName = updateUserDto.LastName;
        }

        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.JobTitle = updateUserDto.JobTitle;
        user.IsEnabled = updateUserDto.IsEnabled;
        user.UpdatedAt = DateTime.UtcNow;

        if (!string.IsNullOrEmpty(updateUserDto.Role))
        {
            var currentRoles = await _userRepository.GetUserRolesAsync(user);
            var removeRolesResult = await _userRepository.RemoveUserFromRolesAsync(user, currentRoles);
            if (!removeRolesResult.Succeeded)
            {
                foreach (var error in removeRolesResult.Errors)
                {
                    ModelState.AddModelError("RoleRemovalError", error.Description);
                }
                return BadRequest(ModelState);
            }

            if (!await _userRepository.RoleExistsAsync(updateUserDto.Role))
            {
                ModelState.AddModelError("RoleNotFound", $"Role '{updateUserDto.Role}' does not exist.");
                return BadRequest(ModelState);
            }

            var addRoleResult = await _userRepository.AddUserToRoleAsync(user, updateUserDto.Role);
            if (!addRoleResult.Succeeded)
            {
                foreach (var error in addRoleResult.Errors)
                {
                    ModelState.AddModelError("RoleAdditionError", error.Description);
                }
                return BadRequest(ModelState);
            }
        }

        var result = await _userRepository.UpdateUserAsync(user);
        if (result.Succeeded)
        {
            return Ok($"Id: {id} Updated ");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("UpdateError", error.Description);
        }

        return BadRequest(ModelState);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound($"{id} Not Found");
        }

        var result = await _userRepository.DeleteUserAsync(user);
        if (result.Succeeded)
        {
            return Ok($"User Id: {id} Deleted Successfully");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return BadRequest(ModelState);
    }
}


//addsecruit  