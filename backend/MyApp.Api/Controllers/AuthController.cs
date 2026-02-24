using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Models;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Change to false when you want to stop the "Cheat" mode
        private const bool IsDevMode = true; 

        // Temporary in-memory list of users
        private static readonly List<UserProfile> _users = new List<UserProfile>();

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try 
            {
                if (string.IsNullOrEmpty(request.Provider))
                    return BadRequest("Provider is required.");

                if (IsDevMode)
                {
                    // Generate a fake external ID based on the button clicked
                    string fakeExternalId = $"dev_{request.Provider.ToLower()}_123";
                    
                    return ProcessLogin(
                        externalId: fakeExternalId, 
                        provider: request.Provider, 
                        email: $"dev-{request.Provider.ToLower()}@siemens-test.com"
                    );
                }

                // LIVE MODE: Future expansion for real Microsoft/Google tokens
                return Unauthorized(new { message = "Live authentication is not yet configured." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        private IActionResult ProcessLogin(string externalId, string provider, string email)
        {
            // 1. Check if user already exists
            var existingUser = _users.FirstOrDefault(u => u.Id == externalId);
            if (existingUser != null) return Ok(existingUser);

            // 2. Create new user with "siemens_x" AppId logic
            var newUser = new UserProfile
            {
                Id = externalId,
                Provider = provider,
                Email = email,
                AppId = $"siemens_{_users.Count + 1}",
                DisplayName = $"siemens_{_users.Count + 1}"
                // JoinedAt is set automatically by the Model's default
            };

            _users.Add(newUser);
            return Ok(newUser);
        }
    }

    // The "Shape" of the data sent from your Angular AuthService
    public class LoginRequest 
    {
        public string Provider { get; set; } = string.Empty;
    }
}