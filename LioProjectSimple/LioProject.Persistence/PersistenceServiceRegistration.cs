using LioProject.Persistence.Interfaces;
using LioProject.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LioProject.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static void RegisterPersistenceServices( this IServiceCollection services, IConfiguration configuration)
        {
            #region Context Registration
            if (configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                services.AddDbContext<LioProjectDbContext>(options =>
                    options.UseInMemoryDatabase("test_db"));
            }
            else
            {
                services.AddDbContext<LioProjectDbContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("LioProjectConnectionString"),
                    optionsBuilder =>
                    optionsBuilder.MigrationsAssembly("LioProject.MVC"))
                    .UseInternalServiceProvider(
                        new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider()
                        ).UseSqlServer(configuration.GetConnectionString("LioProjectConnectionString"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(2),
                                errorNumbersToAdd: null);
                        })
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                });
            }
            #endregion
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
    }
}