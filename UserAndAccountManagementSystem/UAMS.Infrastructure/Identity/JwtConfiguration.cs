using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UAMS.Infrastructure.Identity
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            try
            {
                var jwtSettings = configuration.GetSection("JwtSettings");
                var publicKeyPath = jwtSettings["PublicKeyPath"]
                    ?? throw new InvalidOperationException("JWT public key path not configured.");

                if (!File.Exists(publicKeyPath))
                    throw new FileNotFoundException($"Public key file not found: {publicKeyPath}");

                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText(publicKeyPath));

                var rsaParameters = rsa.ExportParameters(false);
                var key = new RsaSecurityKey(rsaParameters);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = key,
                        ClockSkew = TimeSpan.Zero // Prevents accepting expired tokens
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"[JWT ERROR] {context.Exception.GetType().Name}: {context.Exception.Message}");
                            Console.ResetColor();
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"[JWT SUCCESS] Token validated for: {context.Principal?.Identity?.Name}");
                            Console.ResetColor();
                            return Task.CompletedTask;
                        }
                    };
                });

                Console.WriteLine("[INFO] JWT Authentication configured successfully.");
                return services;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[FATAL] Error in AddJwtAuthentication: {ex.Message}");
                Console.ResetColor();
                throw;
            }
        }
    }
}
