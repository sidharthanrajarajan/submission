using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

            return services;
        }
    }
}