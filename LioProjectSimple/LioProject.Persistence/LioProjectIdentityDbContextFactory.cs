using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LioProject.Domain.Entities;
using System.IO;

namespace LioProject.Persistence
{
    /// <summary>
    /// Classe de fabrique pour la création de contextes de base de données LeaveManagement.
    /// </summary>
    public class SBCommerceDbContextFactory : IDesignTimeDbContextFactory<LioProjectDbContext>
    {
        /// <summary>
        /// Crée un nouveau contexte de base de données LeaveManagement.
        /// </summary>
        /// <param name="args">Les arguments passés à la méthode.</param>
        /// <returns>Un nouveau contexte de base de données LeaveManagement.</returns>
        public LioProjectDbContext CreateDbContext(string[] args)
        {
            try
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                    .AddEnvironmentVariables();

                IConfigurationRoot configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("SBCommerceConnectionString");

                var optionsBuilder = new DbContextOptionsBuilder<LioProjectDbContext>()
                    .UseSqlServer(connectionString);

                return new LioProjectDbContext(optionsBuilder.Options);
            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                // Throw a new exception or return null as appropriate for your application
                throw new Exception("An error occurred while creating the database context.", ex);
            }
        }
    }
}
