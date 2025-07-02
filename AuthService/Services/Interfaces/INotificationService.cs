using AuthService.Models;

namespace AuthService.Services.Interfaces
{
	public interface INotificationService
	{
		Task<AuthResponse> SendPasswordResetEmailAsync(string email);
		Task<AuthResponse> SendEmailVerificationAsync(string userId);
	}

}
