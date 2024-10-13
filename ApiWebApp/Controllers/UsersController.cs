using ApiWebApp.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ApiWebApp.DAL.Model;
using ApiWebApp.Mapping;

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
            userList.Add(UsersMap.ToDto(user, roles));
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
            return Ok(UsersMap.ToDto(user, roles));
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

            var user = UsersMap.ToModel(userDto);
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

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, UsersMap.ToDto(user, new List<string> { userDto.Role }));
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
            user.ProfileImage = new Image();
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

            return Ok(UsersMap.ToDto(updatedUser, roles));
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
