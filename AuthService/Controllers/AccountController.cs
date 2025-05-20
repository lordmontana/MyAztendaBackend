using AuthService.Persistence;
using AuthService.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
		private readonly ITokenService _tokenService;

		public AccountController( IDistributedCache cache, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,ITokenService tokenService)
        {
            _cache = cache;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        // Register endpoint
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Add custom claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim("sub", user.Id),  // Custom claim for 'sub'
                    // Add any other claims you need here
                };

                var addClaimsResult = await _userManager.AddClaimsAsync(user, claims);
                if (!addClaimsResult.Succeeded)
                {
                    return BadRequest("Failed to add claims to the user.");
                }

                // Optionally, sign in the user after registration
                await _signInManager.SignInAsync(user, isPersistent: false);

                return Ok(new { Message = "User registered successfully." });
            }


            return BadRequest(result.Errors);
        }


		// Login endpoint
		[HttpPost("login")]

        public async Task<string> Login(string username, string password)
        {
            // Validate the user's credentials
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                throw new Exception("User not found.");

            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
                throw new Exception("Invalid credentials.");

            // Generate the JWT token
          //  var token = GenerateJwtToken(user);
			var token = _tokenService.GenerateJwtToken(user);

			#region Enable When Redis Server Available

			//var authService = new AuthService();
			//// Store the session info in Redis

			//authService.StoreJwtTokenInRedis(user.Id, token); 
			// Store in Redis with an expiration time


			#endregion
			return token; // Return the token for the user
        }
        private string GenerateJwtToken(ApplicationUser user)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            var claims = new[]
            {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
              };

            var key = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5000",
                audience: "https://localhost:5000",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    public class RegisterModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    } 
}
