namespace AuthService.Models
{
	public class AuthResponse
	{
		public UserDto? User { get; set; }
		public SessionDto? Session { get; set; }
		public string? WeakPassword { get; set; }
		public string? Error { get; set; }
	}
}
