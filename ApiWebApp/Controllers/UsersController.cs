using ApiWebApp.Dto;
using ApiWebApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<AppUsers> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

      [EnableCors("*")]
      [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
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

        [EnableCors("*")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
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
                    Roles= roles,
                });
            }
            return NotFound();
        }

        [HttpPost]
        
        public async Task<IActionResult> Register([FromBody] RegiterUserDto registerUserDto)
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
            {   FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                UserName = registerUserDto.Email,
                Email = registerUserDto.Email,
                PhoneNumber = registerUserDto.PhoneNumber,
                DateOfBirth = registerUserDto.DateOfBirth,
                JobTitle = registerUserDto.JobTitle,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
               
               
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);
            if (result.Succeeded)
            {
                // Check if the role exists
                if (!await _roleManager.RoleExistsAsync(registerUserDto.Role))
                {
                    return BadRequest($"Role '{registerUserDto.Role}' does not exist.");
                }

                // Assign role to the user
                await _userManager.AddToRoleAsync(user, registerUserDto.Role);

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
                   
                    
                   
                });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id);
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

            // Update roles if Role is provided
            if (!string.IsNullOrEmpty(updateUserDto.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    foreach (var error in removeRolesResult.Errors)
                    {
                        ModelState.AddModelError("RoleRemovalError", error.Description);
                    }
                    return BadRequest(ModelState);
                }

                if (!await _roleManager.RoleExistsAsync(updateUserDto.Role))
                {
                    ModelState.AddModelError("RoleNotFound", $"Role '{updateUserDto.Role}' does not exist.");
                    return BadRequest(ModelState);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, updateUserDto.Role);
                if (!addRoleResult.Succeeded)
                {
                    foreach (var error in addRoleResult.Errors)
                    {
                        ModelState.AddModelError("RoleAdditionError", error.Description);
                    }
                    return BadRequest(ModelState);
                }
            }

            var result = await _userManager.UpdateAsync(user);
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
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"{id} Not Found");
            }

            var result = await _userManager.DeleteAsync(user);
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
}
