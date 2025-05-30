using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Nexttech.Models;


namespace Nexttech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]  // Apply role requirement to all endpoints here
    public class AdminController : ControllerBase
    {
        private readonly UserManager<NexttechUser> _userManager;

        public AdminController(UserManager<NexttechUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("admin")]
        public IActionResult AdminEndpoint()
        {
            return Ok(new { message = "You are an Admin" });
        }
        
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users.Cast<NexttechUser>())
            {
                // Optional: Backend logging
                Console.WriteLine($"User: {user.Email} | FirstName: {user.FirstName} | LastName: {user.LastName}");

                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = roles.ToList()
                });
            }

            return Ok(userDtos);
        }


        // Delete a user by ID
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new { message = "User deleted successfully" });

            return BadRequest(result.Errors);
        }
        [HttpGet("users/{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpPost("users/assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.AddToRoleAsync(user, dto.Role);
            return result.Succeeded ? Ok("Role assigned") : BadRequest(result.Errors);
        }

        [HttpPost("users/remove-role")]
        public async Task<IActionResult> RemoveRole([FromBody] UserRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound("User not found");

            var result = await _userManager.RemoveFromRoleAsync(user, dto.Role);
            return result.Succeeded ? Ok("Role removed") : BadRequest(result.Errors);
        }
    }
}
