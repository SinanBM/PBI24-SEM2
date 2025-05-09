// Controllers/AuthController.cs

using Microsoft.AspNetCore.Mvc; // Giver adgang til MVC-funktioner som ControllerBase
using user.cs; 
using DataUserRepository.cs;

namespace NexttechCalc.Controllers // ← Brug dit projekt- eller mappenavn
{
    [ApiController] // Fortæller systemet, at dette er en API-controller (til fx REST API)
    [Route("api/[controller]")] // Denne controller svarer på kald som: /api/auth
    public class AuthController : ControllerBase // ControllerBase giver adgang til API-responsmetoder
    {
        // Denne metode kaldes, når nogen sender en POST-request til /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            // Tjek om brugeren har skrevet både email og password
            if (string.IsNullOrEmpty(loginUser.Email) || string.IsNullOrEmpty(loginUser.Password))
            {
                return BadRequest("E-mail og adgangskode skal udfyldes."); // HTTP 400
            }

            // Find brugeren i den midlertidige "mock-database"
            var user = MockUserRepository.GetUserByEmail(loginUser.Email);

            // Hvis vi ikke finder brugeren, eller password ikke stemmer
            if (user == null || user.Password != loginUser.Password)
            {
                return Unauthorized("Forkert e-mail eller adgangskode."); // HTTP 401
            }

            // Hvis login er korrekt
            return Ok("Du er nu logget ind!"); // HTTP 200
        }
    }
}