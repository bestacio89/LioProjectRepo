using LioProject.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace LioProject.MVC.Helpers
{
    public class SysHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<SysHelper> _logger;

        public SysHelper( UserManager<ApplicationUser> userManager,  IHttpContextAccessor httpContextAccessor, ILogger<SysHelper> logger)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<string> ShowCurrentUser()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            if (user != null)
            {
                return $"{user.FirstName} {user.LastName}";
            }

            // Handle the case when the user is not found or not authenticated.
            return string.Empty;
        }

        public async Task<ApplicationUser> GetCurrentUser()
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Test")
            {
                return new ApplicationUser { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            }

            try
            {
                var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in retrieving the current user: {ex.Message}");
                throw;
            }
        }
    }
}
