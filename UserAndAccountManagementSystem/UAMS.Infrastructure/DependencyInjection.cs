using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UAMS.Infrastructure.Data;
using UAMS.Infrastructure.Identity;
using UAMS.Infrastructure.Persistence;

namespace UAMS.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnectionString");

            services.AddDbContext<UamsDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //register
            services.AddJwtAuthentication(configuration);
            services.AddScoped<JwtTokenService>();

            return services;
        }
    }
}