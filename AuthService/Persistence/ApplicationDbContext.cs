using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }


        protected override void OnModelCreating(ModelBuilder b)
        {
            b.HasDefaultSchema("atzenda");   // for postgres
            base.OnModelCreating(b);

        }
    } 
}