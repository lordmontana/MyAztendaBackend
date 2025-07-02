using AuthService.Models;

namespace AuthService.Services.Interfaces
{
	public interface IAuthService
	{
		Task<AuthResponse> LoginAsync(LoginModel request);
		Task<AuthResponse> LogoutAsync(string userId);
		Task<AuthResponse> RegisterAsync(LoginModel request);
		Task<AuthResponse> RefreshTokenAsync(string refreshToken);
		Task<AuthResponse> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
		Task<AuthResponse> ResetPasswordAsync(string userId, string newPassword);
		Task<AuthResponse> VerifyEmailAsync(string userId, string token);
	}
}
