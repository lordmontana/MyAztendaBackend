namespace AuthService.Models
{
	public class SessionDto
	{
		public string AccessToken { get; set; } = string.Empty;
		public int ExpiresIn { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
