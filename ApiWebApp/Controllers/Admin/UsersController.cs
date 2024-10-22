using ApiWebApp.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ApiWebApp.Mapping;
using Microsoft.AspNetCore.Identity;
using DAL.Models.UsersModel;
using Microsoft.AspNetCore.Authorization;

namespace ApiWebApp.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
    public class UsersController(IUserRepository userRepository) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;

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
            if (user is not null)
            {
                var roles = await _userRepository.GetUserRolesAsync(user);
                return Ok(UsersMap.ToDto(user, roles));
            }
            return NotFound();
        }

        [HttpPost]
        
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
            if (user is null)
            {
                ModelState.AddModelError("UserNotFound", "User with the specified ID was not found.");
                return NotFound(ModelState);
            }

            // Use UsersMap to update user properties
            UsersMap.UpdateModel(user, updateUserDto);

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

        [HttpPatch("{id}/reset-password")]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = "Manager")]
        public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                return NotFound(ModelState);
            }

            var passwordHasher = new PasswordHasher<AppUsers>();
            user.PasswordHash = passwordHasher.HashPassword(user, resetPasswordDto.Password);

            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return Ok();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("UpdateError", error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPatch("{id}/update-status")]
        public async Task<IActionResult> UpdateUserStatus(string id, [FromBody] UpdateUserStatusDto updateUserStatusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                ModelState.AddModelError("UserNotFound", "User with the specified ID was not found.");
                return NotFound(ModelState);
            }

            user.IsEnabled = updateUserStatusDto.IsEnabled;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                // Return the updated status
                return Ok(new { user.IsEnabled });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("UpdateError", error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPatch("{id}/update-JobTitle")]
        public async Task<IActionResult> UpdateProfile(string id, [FromBody] UserJobTitleDto updateUserJobDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null)
            {
                ModelState.AddModelError("UserNotFound", "User with the specified ID was not found.");
                return NotFound(ModelState);
            }

            user.JobTitle = updateUserJobDto.JobTitle;
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userRepository.UpdateUserAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { user.JobTitle });
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
                return NoContent();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }
    }
}
