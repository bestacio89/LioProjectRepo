using Microsoft.EntityFrameworkCore;
using LioProject.Domain.Entities; // Replace with the correct namespace for your ApplicationUser class

namespace LioProject.Persistence
{
    public class LioProjectDbContext : DbContext
    {
        public LioProjectDbContext(DbContextOptions<LioProjectDbContext> options) : base(options)
        {
        }


    }
  
}


