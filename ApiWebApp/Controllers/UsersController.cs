using ApiWebApp.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ApiWebApp.DAL.Model;

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
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Reviewer")]
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
                Roles = roles,
                user.CreatedAt,
                user.UpdatedAt,
                ProfileImage = new
                {
                    Alt = user.ProfileImage?.Alt ?? string.Empty,
                    Src = user.ProfileImage?.Src ?? string.Empty
                }
            });
        }

        return Ok(userList);
    }

    [EnableCors("AllowAll")]
    [HttpGet("{id}")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Reviewer")]
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
                user.IsEnabled,
                user.CreatedAt,
                user.UpdatedAt,
                Roles = roles,
                ProfileImage = new
                {
                    Alt = user.ProfileImage?.Alt ?? string.Empty,
                    Src = user.ProfileImage?.Src ?? string.Empty
                }
            });
        }
        return NotFound();
    }

    [HttpPost]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
    public async Task<IActionResult> Register([FromBody] UserDto userDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUsers
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.Email,
                Email = userDto.Email,
                PhoneNumber = userDto.PhoneNumber,
                DateOfBirth = userDto.DateOfBirth,
                JobTitle = userDto.JobTitle ?? string.Empty,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                ProfileImage = new ProfileImage
                {
                    Alt = userDto.ProfileAlt ?? string.Empty,
                    Src = userDto.ProfileSrc ?? string.Empty
                }
            };

            var result = await _userRepository.CreateUserAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                if (!await _userRepository.RoleExistsAsync(userDto.Role))
                {
                    return BadRequest($"Role '{userDto.Role}' does not exist.");
                }

                var roleResult = await _userRepository.AddUserToRoleAsync(user, userDto.Role);
                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError("RoleAssignmentError", error.Description);
                    }
                    return BadRequest(ModelState);
                }

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
                {
                    user.Id,
                    user.Email,
                    user.PhoneNumber,
                    user.FirstName,
                    user.LastName,
                    user.DateOfBirth,
                    user.JobTitle,
                    user.UserName,
                    user.IsEnabled,
                    user.CreatedAt,
                    user.UpdatedAt,
                    ProfileImage = new
                    {
                        Alt = user.ProfileImage?.Alt ?? string.Empty,
                        Src = user.ProfileImage?.Src ?? string.Empty
                    }
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
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager,Reviewer")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UserDto updateUserDto)
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

        // Update user properties if they are provided in the DTO
        user.FirstName = updateUserDto.FirstName ?? user.FirstName;
        user.LastName = updateUserDto.LastName ?? user.LastName;
        user.PhoneNumber = updateUserDto.PhoneNumber ?? user.PhoneNumber;
        user.JobTitle = updateUserDto.JobTitle ?? user.JobTitle;
        user.IsEnabled = updateUserDto.IsEnabled;
        user.UpdatedAt = DateTime.UtcNow;

        // Update ProfileImage properties if they are provided in the DTO
        if (user.ProfileImage == null)
        {
            user.ProfileImage = new ProfileImage();
        }
        user.ProfileImage.Alt = updateUserDto.ProfileAlt ?? user.ProfileImage.Alt;
        user.ProfileImage.Src = updateUserDto.ProfileSrc ?? user.ProfileImage.Src;

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
            // Fetch the updated user from the database
            var updatedUser = await _userRepository.GetUserByIdAsync(id);
            var roles = await _userRepository.GetUserRolesAsync(updatedUser);

            return Ok(new
            {
                updatedUser.Id,
                updatedUser.Email,
                updatedUser.PhoneNumber,
                updatedUser.FirstName,
                updatedUser.LastName,
                updatedUser.DateOfBirth,
                updatedUser.JobTitle,
                updatedUser.IsEnabled,
                updatedUser.CreatedAt,
                updatedUser.UpdatedAt,
                Roles = roles,
                ProfileImage = new
                {
                    Alt = updatedUser.ProfileImage?.Alt ?? string.Empty,
                    Src = updatedUser.ProfileImage?.Src ?? string.Empty
                }
            });
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError("UpdateError", error.Description);
        }

        return BadRequest(ModelState);
    }


    [HttpDelete("{id}")]
    //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
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
