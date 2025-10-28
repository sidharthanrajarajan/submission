using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UAMS.Infrastructure.Data;
using UAMS.Infrastructure.Identity;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var connectionString = configuration.GetConnectionString("DatabaseConnectionString");

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Database connection string is not configured properly.");

                // Register DbContexts with custom EF Migrations history table
                services.AddDbContext<UamsDbContext>(options =>
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "training");
                    }));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "training");
                    }));

                // Add Identity
                services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

                // Register JWT authentication & related services
                services.AddJwtAuthentication(configuration);
                services.AddScoped<JwtTokenService>();

                // Log success message
                var serviceProvider = services.BuildServiceProvider();
                var logger = serviceProvider.GetService<ILoggerFactory>()?.CreateLogger("DependencyInjection");
                logger?.LogInformation("Infrastructure services successfully configured.");

                return services;
            }
            catch (InvalidOperationException ex)
            {
                Console.Error.WriteLine($"Configuration error: {ex.Message}");
                throw;
            }
            catch (DbUpdateException dbEx)
            {
                Console.Error.WriteLine($"Database configuration error: {dbEx.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An unexpected error occurred during infrastructure setup: {ex.Message}");
                throw;
            }
        }
    }
}
