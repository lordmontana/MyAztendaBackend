using Microsoft.EntityFrameworkCore;
using LocationService.Entities;
using System.Collections.Generic;

namespace LocationService.Data;

public class ApplicationDbContext : DbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public DbSet<Location> Location => Set<Location>();
}