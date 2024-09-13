using ApiWebApp.Dto;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [EnableCors("*")]
        [HttpGet]
      
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }
        [EnableCors("*")]
        [HttpGet("{id}")]
     
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegiterUserDto regiterUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (regiterUserDto.Password != regiterUserDto.ConfirmPassword)
            {
                ModelState.AddModelError("Password", "The password and confirmation password do not match.");
                return BadRequest(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = regiterUserDto.Email,
                Email = regiterUserDto.Email,
                PhoneNumber = regiterUserDto.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, regiterUserDto.Password);
            if (result.Succeeded)
            {
                // Check if the role exists
                if (!await _roleManager.RoleExistsAsync(regiterUserDto.Role))
                {
                    return BadRequest($"Role '{regiterUserDto.Role}' does not exist.");
                }

                // Assign role to the user
                await _userManager.AddToRoleAsync(user, regiterUserDto.Role);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new
                {
                    user.Id,
                    user.Email,
                    user.PhoneNumber
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
                return NotFound();
            }

            user.PhoneNumber = updateUserDto.PhoneNumber;
          

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok($"Id: {id} Updated ");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
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
                // Delete the user id Deleted Sucssesfuly
            return Ok($"User Id: {id} Deleted Sucssesfuly");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return BadRequest(ModelState);
    
}
    }

}

//to prevent ssrf attacks, you should validate the url before making a request to it.


