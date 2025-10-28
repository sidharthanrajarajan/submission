using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using UAMS.Infrastructure;

namespace UAMS.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddOpenApi();

            // Infrastructure setup (DbContext, Identity, JwtTokenService)
            builder.Services.AddInfrastructure(builder.Configuration);

            var jwtSettings = builder.Configuration.GetSection("JwtSettings");
            var publicKeyPath = jwtSettings["PublicKeyPath"];

            if (string.IsNullOrEmpty(publicKeyPath) || !File.Exists(publicKeyPath))
                throw new FileNotFoundException("Public key file not found at path: " + publicKeyPath);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(publicKeyPath));

            var rsaSecurityKey = new RsaSecurityKey(rsa);

            builder.Services
                .AddAuthentication(options =>
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
                        IssuerSigningKey = rsaSecurityKey
                    };
                });

            var app = builder.Build();

            // Configure HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
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
