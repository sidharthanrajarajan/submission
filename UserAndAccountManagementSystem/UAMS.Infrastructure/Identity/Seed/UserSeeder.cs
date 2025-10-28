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
            string[] roles = { "Admin", "Employee", "Customer" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    logger.LogInformation("Role '{Role}' created successfully.", role);
                }
            }

            // Admin user
            await EnsureUserAsync(userManager, "admin", "admin@uams.com", "Admin@123", "Admin", logger);

            // Employee user
            await EnsureUserAsync(userManager, "employee", "employee@uams.com", "Employee@123", "Employee", logger);

            // Customer user
            await EnsureUserAsync(userManager, "customer", "customer@uams.com", "Customer@123", "Customer", logger);
        }

        private static async Task EnsureUserAsync(
            UserManager<ApplicationUser> userManager,
            string username,
            string email,
            string password,
            string role,
            ILogger logger)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user != null)
            {
                logger.LogInformation("User '{User}' already exists. Skipping creation.", username);

                // Ensure user has the role (in case seeding ran before)
                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                    logger.LogInformation("Added existing user '{User}' to role '{Role}'.", username, role);
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

            await userManager.AddToRoleAsync(user, role);
            logger.LogInformation("User '{User}' created successfully and added to role '{Role}'.", username, role);
        }
    }
}