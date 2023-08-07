using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace LioProject.Identity
{
    public class IdentityContextFactory : IDesignTimeDbContextFactory<LioProjectIdentityDbContext>
    {
        public LioProjectIdentityDbContext CreateDbContext(string[] args)
        {
            // Get the current directory as the base path.
            string basePath = Directory.GetCurrentDirectory();

            // Build the configuration from appsettings.json or other configuration files.
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .Build();

            // Get the connection string for the LioProjectIdentityDbContext from the configuration.
            string connectionString = configuration.GetConnectionString("LioProjectIdentityConnection");

            // Create options for the LioProjectIdentityDbContext with the specified connection string.
            DbContextOptionsBuilder<LioProjectIdentityDbContext> optionsBuilder = new DbContextOptionsBuilder<LioProjectIdentityDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Return a new instance of the LioProjectIdentityDbContext with the specified options.
            return new LioProjectIdentityDbContext(optionsBuilder.Options);
        }
    }
}
