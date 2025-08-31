using System.Security.Claims;
using AuthService.Models;
using AuthService.Persistence;
using AuthService.Services;
using AuthService.Services.Interfaces;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly IAuthService _authService;


		public AccountController( IDistributedCache cache, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration,ITokenService tokenService, IAuthService authService)
        {
            _cache = cache;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _tokenService = tokenService;
			_authService = authService;

		}

		// Register endpoint
		[HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _authService.RegisterAsync(model);
			if (!string.IsNullOrEmpty(response.Error))
				return BadRequest(new { response.Error });

			return Ok(response);
		}

                // Login endpoint
                [HttpPost("login")]
                public async Task<IActionResult> Login([FromBody] LoginModel request)
                {
                        var response = await _authService.LoginAsync(request);
                        if (response.Error != null) return Unauthorized(response);
                        return Ok(response);

                }

                // Refresh token endpoint
                [HttpPost("refresh")]
                public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
                {
                        var response = await _authService.RefreshTokenAsync(request.RefreshToken);
                        if (response.Error != null) return Unauthorized(response);
                        return Ok(response);
                }
       

    }

 


}
