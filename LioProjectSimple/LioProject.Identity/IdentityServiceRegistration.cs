using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LioProject.Identity;
using LioProject.Domain.Users; // Replace with the correct namespace for ApplicationUser
using LioProject.Identity.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LioProject.Identity
{ 
    public static class PersistenceServiceRegistration
    {
        public static void AddCustomIdentity(this IServiceCollection services, IConfiguration Configuration)
        {
            // Register IdentityDbContext and other Identity-related services
            services.AddDbContext<LioProjectIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LioProjectIdentityConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<LioProjectIdentityDbContext>()
                .AddDefaultTokenProviders();

            // Add your custom password validator, if needed
            services.Replace(ServiceDescriptor.Singleton<IPasswordValidator<ApplicationUser>, CustomPasswordValidator>());
        }
    }
}
