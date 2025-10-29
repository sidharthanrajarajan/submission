using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using UAMS.Infrastructure;
using UAMS.Infrastructure.Identity;
using UAMS.Infrastructure.Identity.Seed;
using UAMS.Domain.Entities;

namespace UAMS.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();

            // Swagger + JWT
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new() { Title = "UAMS API", Version = "v1" });

                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter your token after bearer.\n\nExample: **Bearer eyJhbGciOi...**"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddInfrastructure(builder.Configuration);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var publicKeyPath = jwtSettings["PublicKeyPath"];

            if (string.IsNullOrEmpty(publicKeyPath) || !File.Exists(publicKeyPath))
                throw new FileNotFoundException("Public key file not found at path: " + publicKeyPath);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(publicKeyPath));
            var rsaSecurityKey = new RsaSecurityKey(rsa);

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var logger = services.GetRequiredService<ILogger<Program>>();
                await UserSeeder.SeedDefaultUsersAsync(userManager, roleManager, logger);
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
