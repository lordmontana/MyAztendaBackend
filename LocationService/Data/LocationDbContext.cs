using Microsoft.EntityFrameworkCore;
using LocationService.Entities;
using System.Collections.Generic;

namespace LocationService.Data;

public class LocationDbContext : DbContext
{
	public LocationDbContext(DbContextOptions<LocationDbContext> options) : base(options) { }

	public DbSet<Location> Locations => Set<Location>();
}