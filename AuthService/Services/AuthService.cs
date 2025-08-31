using System.Security.Claims;
using System.Security.Cryptography;
using AuthService.Models;
using AuthService.Persistence;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace AuthService.Services
{
	public class AuthService : IAuthService , IUserService, INotificationService
	{
                private readonly UserManager<ApplicationUser> _userManager;
                private readonly SignInManager<ApplicationUser> _signInManager;
                private readonly ITokenService _tokenService;
                private readonly JwtSettingsModel _jwtSettings;
                private readonly IDistributedCache _cache;

		public AuthService(
                        UserManager<ApplicationUser> userManager,
                        SignInManager<ApplicationUser> signInManager,
                        ITokenService tokenService,
                        IOptions<JwtSettingsModel> jwtOptions,
                        IDistributedCache cache)
	
		{
                        _userManager = userManager;
                        _signInManager = signInManager;
                        _tokenService = tokenService;
                        _jwtSettings = jwtOptions.Value;
                        _cache = cache;

                }

		public async Task<AuthResponse> LoginAsync(LoginModel request)
		{
			var user = await _userManager.FindByNameAsync(request.Username);
			if (user == null)
			{
				return new AuthResponse
				{
					Error = "User not found."
				};
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
			if (!result.Succeeded)
			{
				return new AuthResponse
				{
					Error = "Invalid credentials."
				};
			}

                        var token = _tokenService.GenerateJwtToken(user);
                        var refreshToken = GenerateRefreshToken();
                        await StoreRefreshTokenAsync(refreshToken, user.Id);

                        return CreateAuthResponse(user, token, refreshToken, request.Password);
                }


		public async Task<AuthResponse> RegisterAsync(RegisterModel request)
		{
			var existingUser = await _userManager.FindByNameAsync(request.Username);
			if (existingUser != null)
			{
				return new AuthResponse
				{
					Error = "Username is already taken."
				};
			}

			var user = new ApplicationUser
			{
				UserName = request.Username,
				Email = request.Username // Or request.Email if you're using RegisterModel
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				return new AuthResponse
				{
					Error = string.Join(", ", result.Errors.Select(e => e.Description))
				};
			}

			// Add claims
			var claims = new List<Claim>
			{
			new Claim(ClaimTypes.Name, user.UserName),
			new Claim("sub", user.Id),
			new Claim("installationId", user.IId.ToString() ?? string.Empty)// edw prepei na mpei pinakas licenses or select max(iid) from users 

            };

			var claimsResult = await _userManager.AddClaimsAsync(user, claims);
			if (!claimsResult.Succeeded)
			{
				return new AuthResponse
				{
					Error = "Failed to add claims."
				};
			}

			// Optional: Sign in the user
			await _signInManager.SignInAsync(user, isPersistent: false);

                        // Generate token
                        var token = _tokenService.GenerateJwtToken(user);
                        var refreshToken = GenerateRefreshToken();
                        await StoreRefreshTokenAsync(refreshToken, user.Id);

                        return CreateAuthResponse(user, token, refreshToken, request.Password);

		}
                private AuthResponse CreateAuthResponse(ApplicationUser user, string token, string refreshToken, string? originalPassword = null)
                {
                        return new AuthResponse
                        {
                                User = new UserDto(user),
                                Session = new SessionDto
                                {
                                        AccessToken = token,
                                        RefreshToken = refreshToken,
                                        ExpiresIn = _jwtSettings.ExpireMinutes * 60,
                                        CreatedAt = DateTime.UtcNow
                                },
                                WeakPassword = originalPassword != null && originalPassword.Length < 6
                ? "Password is weak"
                : null
                        };
                }

                private static string GenerateRefreshToken()
                {
                        var bytes = new byte[32];
                        RandomNumberGenerator.Fill(bytes);
                        return Convert.ToBase64String(bytes);
                }

                private async Task StoreRefreshTokenAsync(string refreshToken, string userId)
                {
                        var options = new DistributedCacheEntryOptions
                        {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                        };
                        await _cache.SetStringAsync(refreshToken, userId, options);
                }

                // Other methods not implemented yet
                public Task<AuthResponse> LogoutAsync(string userId) => throw new NotImplementedException();
                public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
                {
                        var userId = await _cache.GetStringAsync(refreshToken);
                        if (string.IsNullOrEmpty(userId))
                        {
                                return new AuthResponse
                                {
                                        Error = "Invalid refresh token."
                                };
                        }

                        var user = await _userManager.FindByIdAsync(userId);
                        if (user == null)
                        {
                                return new AuthResponse
                                {
                                        Error = "User not found."
                                };
                        }

                        await _cache.RemoveAsync(refreshToken);
                        var newToken = _tokenService.GenerateJwtToken(user);
                        var newRefreshToken = GenerateRefreshToken();
                        await StoreRefreshTokenAsync(newRefreshToken, user.Id);

                        return CreateAuthResponse(user, newToken, newRefreshToken);
                }
		public Task<AuthResponse> GetUserAsync(string userId) => throw new NotImplementedException();
		public Task<AuthResponse> UpdateUserAsync(string userId, LoginModel request) => throw new NotImplementedException();
		public Task<AuthResponse> DeleteUserAsync(string userId) => throw new NotImplementedException();
		public Task<AuthResponse> ChangePasswordAsync(string userId, string oldPassword, string newPassword) => throw new NotImplementedException();
		public Task<AuthResponse> ResetPasswordAsync(string userId, string newPassword) => throw new NotImplementedException();
		public Task<AuthResponse> VerifyEmailAsync(string userId, string token) => throw new NotImplementedException();
		public Task<AuthResponse> SendPasswordResetEmailAsync(string email) => throw new NotImplementedException();
		public Task<AuthResponse> SendEmailVerificationAsync(string userId) => throw new NotImplementedException();
		public Task<AuthResponse> GetUserByEmailAsync(string email) => throw new NotImplementedException();
		public Task<AuthResponse> GetUserByUsernameAsync(string username) => throw new NotImplementedException();
		public Task<AuthResponse> GetUserByLicenseIdAsync(int licenseId) => throw new NotImplementedException();
		public Task<AuthResponse> GetUserByIdAsync(string userId) => throw new NotImplementedException();
		public Task<AuthResponse> GetAllUsersAsync(int pageNumber, int pageSize) => throw new NotImplementedException();
	}
}	

