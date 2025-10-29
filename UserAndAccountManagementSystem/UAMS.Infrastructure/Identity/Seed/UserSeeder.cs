using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using UAMS.Domain.Entities;

namespace UAMS.Infrastructure.Identity.Seed
{
    public static class UserSeeder
    {
        public static async Task SeedDefaultUsersAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger logger)
        {
            string[] roles = { "Admin", "Employee", "Customer"};

            // Ensure all roles exist
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Role '{Role}' created successfully.", role);
                }
            }

            await EnsureUserAsync(userManager, "admin", "admin@uams.com", "Admin@123", new[] { "Admin" }, logger);

            await EnsureUserAsync(userManager, "employee", "employee@uams.com", "Employee@123", new[] { "Employee" }, logger);

            await EnsureUserAsync(userManager, "customer", "customer@uams.com", "Customer@123", new[] { "Customer" }, logger);

            await EnsureUserAsync(userManager, "auditor", "auditor@uams.com", "Auditor@123",
                new[] { "Employee", "Customer" }, logger);
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string username,
            string email,
            string password,
            string[] roles,
            ILogger logger)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                logger.LogInformation("User '{User}' already exists. Ensuring roles...", username);

                // Ensure user has all assigned roles
                foreach (var role in roles)
                {
                    if (!await userManager.IsInRoleAsync(user, role))
                    {
                        await userManager.AddToRoleAsync(user, role);
                        logger.LogInformation("Added existing user '{User}' to role '{Role}'.", username, role);
                    }
                }
                return;
            }

            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                logger.LogError("Failed to create user '{User}': {Errors}", username, errors);
                return;
            }

            await userManager.AddToRolesAsync(user, roles);
            logger.LogInformation("User '{User}' created successfully with roles: {Roles}.", username, string.Join(", ", roles));
        }
    }
}
