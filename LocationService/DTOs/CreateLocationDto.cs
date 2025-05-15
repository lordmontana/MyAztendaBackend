namespace LocationService.DTOs
{
	public class CreateLocationDto
	{
		public string Name { get; set; } = string.Empty;
		public string Region { get; set; } = string.Empty;
		public int ClientId { get; set; }
	}
}
