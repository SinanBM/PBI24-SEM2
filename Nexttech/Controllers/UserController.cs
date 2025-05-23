using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nexttech.Models;
using System.Security.Claims;

namespace Nexttech.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // All actions require authorization by default
    public class UserController : ControllerBase
    {
        private readonly UserManager<NexttechUser> _userManager;

        public UserController(UserManager<NexttechUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping() => Ok("pong");


        // === Current logged-in user actions ===
        [HttpGet("me")]
        [Authorize] // Require authenticated user
        public async Task<IActionResult> GetMyProfile()
        {
            // Get user ID from the JWT claims
            var userId = User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => c.Value)
                .FirstOrDefault(value => Guid.TryParse(value, out _));

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName
            });
        }

        [Authorize]
        [HttpPost("change-password")]  // or [HttpPost("user/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
                return Ok(new { message = "Password changed successfully." });

            return BadRequest(result.Errors.Select(e => e.Description));
        }




        // === Admin-only user management endpoints ===

        // GET /api/user
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            var userList = new List<object>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Optional: Log to backend console
                Console.WriteLine($"User: {user.Email} | FirstName: {user.FirstName} | LastName: {user.LastName}");

                userList.Add(new {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    Roles = roles
                });
            }

            return Ok(userList);
        }



        // GET /api/user/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                Roles = roles
            });
        }

        // POST /api/user
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var generatedUserName = model.Email.Split('@')[0];
            var user = new NexttechUser
            {
                UserName = generatedUserName,
                Email = model.Email,
                FirstName = model.FirstName,   // <- new required property
                LastName = model.LastName      // <- new required property
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.Select(e => e.Description));

            if (!string.IsNullOrEmpty(model.Role))
            {
                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok(new { user.Id, user.UserName, user.Email });
        }

        // PUT /api/user/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!string.IsNullOrEmpty(model.Email) && model.Email != user.Email)
            {
                var existingEmailUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingEmailUser != null && existingEmailUser.Id != id)
                    return BadRequest(new { message = "Email already in use" });

                user.Email = model.Email;
                user.NormalizedEmail = _userManager.NormalizeEmail(model.Email);
            }
            if (!string.IsNullOrEmpty(model.FirstName))
                user.FirstName = model.FirstName;

            if (!string.IsNullOrEmpty(model.LastName))
                user.LastName = model.LastName;

            if (!string.IsNullOrEmpty(model.UserName) && model.UserName != user.UserName)
            {
                var existingUserName = await _userManager.FindByNameAsync(model.UserName);
                if (existingUserName != null && existingUserName.Id != id)
                    return BadRequest(new { message = "Username already taken" });

                user.UserName = model.UserName;
                user.NormalizedUserName = _userManager.NormalizeName(model.UserName);
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));


            // Update role if provided
            if (!string.IsNullOrEmpty(model.Role))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                await _userManager.AddToRoleAsync(user, model.Role);
            }

            return Ok(new { user.Id, user.UserName, user.Email });
        }

        // DELETE /api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));


            return Ok(new { message = "User deleted" });
        }
    }
}
