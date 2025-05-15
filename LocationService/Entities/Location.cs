﻿namespace LocationService.Entities
{
	public class Location
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Region { get; set; } = string.Empty;
		public int ClientId { get; set; }
	}
}
