using Microsoft.AspNetCore.Mvc;
using MyApp.Api.Models;
using MyApp.Api.Data; // Ensure this matches where your AppDbContext lives
using Microsoft.EntityFrameworkCore;

namespace MyApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // The _context is our "bridge" to the Neon Database
        private readonly AppDbContext _context;
        private const bool IsDevMode = true; 

        // Constructor: ASP.NET "injects" the database connection here automatically
        public AuthController(AppDbContext context)
        {
            _context = context;
        }

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

                return Unauthorized(new { message = "Live authentication is not yet configured." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal error: {ex.Message}");
            }
        }

        private IActionResult ProcessLogin(string externalId, string provider, string email)
        {
            // 1. Check the DATABASE (Neon) if the user already exists
            // We search by ExternalId (the ID from Google/Microsoft/DevMode)
            var existingUser = _context.UserProfiles.FirstOrDefault(u => u.ExternalId == externalId);
            
            if (existingUser != null) 
            {
                return Ok(existingUser);
            }

            // 2. Prepare a new user object
            var newUser = new UserProfile
            {
                ExternalId = externalId,
                Provider = provider,
                Email = email,
                // We leave DisplayName/AppId blank for a moment because 
                // we don't know the unique numeric ID yet until we save.
            };

            // 3. Add to the tracking list and Save to Neon
            _context.UserProfiles.Add(newUser);
            _context.SaveChanges(); 
            
            /* MAGIC MOMENT: 
               After SaveChanges(), Postgres has assigned a unique 'Id' (e.g., 1, 2, 3).
               EF Core automatically updated our 'newUser.Id' property with that value.
            */

            // 4. Now we can apply your "siemens_n" logic using the real DB ID
            newUser.DisplayName = $"siemens_{newUser.Id}";
            
            // Save one more time to update the DisplayName in the database
            _context.SaveChanges();

            return Ok(newUser);
        }
    }

    public class LoginRequest 
    {
        public string Provider { get; set; } = string.Empty;
    }
}