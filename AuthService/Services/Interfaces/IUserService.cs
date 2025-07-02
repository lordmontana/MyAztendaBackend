using AuthService.Models;

namespace AuthService.Services
{
	public interface IUserService
	{
		Task<AuthResponse> GetUserByIdAsync(string userId);
		Task<AuthResponse> GetUserByEmailAsync(string email);
		Task<AuthResponse> GetUserByUsernameAsync(string username);
		Task<AuthResponse> GetUserByLicenseIdAsync(int licenseId);
		Task<AuthResponse> GetAllUsersAsync(int pageNumber, int pageSize);
		Task<AuthResponse> UpdateUserAsync(string userId, LoginModel request);
		Task<AuthResponse> DeleteUserAsync(string userId);
	}
}
