using LioProject.Domain.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LioProject.Identity.Helpers
{

    public class CustomPasswordValidator : IPasswordValidator<ApplicationUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            var errors = new List<IdentityError>();

            // Password length validation.
            if (password.Length < 8)
            {
                errors.Add(new IdentityError { Description = "Password must be at least 8 characters long." });
            }

            // Ensure that the password contains at least one uppercase letter.
            if (!password.Any(char.IsUpper))
            {
                errors.Add(new IdentityError { Description = "Password must contain at least one uppercase letter." });
            }

            // Ensure that the password contains at least one lowercase letter.
            if (!password.Any(char.IsLower))
            {
                errors.Add(new IdentityError { Description = "Password must contain at least one lowercase letter." });
            }

            // Ensure that the password contains at least one non-alphanumeric character.
            if (password.All(char.IsLetterOrDigit))
            {
                errors.Add(new IdentityError { Description = "Password must contain at least one non-alphanumeric character." });
            }

            // If there are any errors, return them as IdentityResult.Failed.
            if (errors.Count > 0)
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }

            // If the password meets the requirements, return IdentityResult.Success.
            return Task.FromResult(IdentityResult.Success);
        }
    }
}