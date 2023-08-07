using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LioProject.Domain.Users; // Replace with the correct namespace for your ApplicationUser class

namespace LioProject.Identity
{
    public class LioProjectIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public LioProjectIdentityDbContext(DbContextOptions<LioProjectIdentityDbContext> options) : base(options)
        {
        }

        DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Customize the Identity data model, if necessary.
            // For example, you can add unique constraints, additional indexes, etc.
        }
    }
  
}


