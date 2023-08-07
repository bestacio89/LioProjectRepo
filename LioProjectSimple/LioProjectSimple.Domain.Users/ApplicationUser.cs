
using Microsoft.AspNetCore.Identity;

namespace LioProject.Domain.Users;
public class ApplicationUser : IdentityUser<int>
{
    // Additional properties specific to your application's user data.
    public string FirstName { get; set; }
    public string LastName { get; set; }
    // ... other properties ...

    // Constructors, methods, or business logic can also be added as needed.
}