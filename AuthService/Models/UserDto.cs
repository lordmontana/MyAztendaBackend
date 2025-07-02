using AuthService.Persistence;

namespace AuthService.Models
{
	public class UserDto
	{
		public string Id { get; set; }
		public string Username { get; set; }
		public string? Email { get; set; }
		public string? Role { get; set; }
		public int? LicenseId { get; set; }

		public UserDto(ApplicationUser user)
		{
			Id = user.Id;
			Username = user.UserName!;
			Email = user.Email;
			//Role = user.Role;         // 
			//LicenseId = user.LicenseId; // Customize based on your model
		}
	}
}
