using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UAMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace UAMS.Infrastructure.Identity
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<JwtTokenService> _logger;

        public JwtTokenService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            ILogger<JwtTokenService> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<(string token, DateTime expires)> GenerateAccessTokenAsync(ApplicationUser user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user), "User cannot be null.");

                var jwtSettings = _configuration.GetSection("JwtSettings");
                var privateKeyPath = jwtSettings["PrivateKeyPath"];

                if (string.IsNullOrWhiteSpace(privateKeyPath))
                    throw new InvalidOperationException("Private key path is missing in JwtSettings.");

                if (!File.Exists(privateKeyPath))
                    throw new FileNotFoundException($"Private key file not found at: {privateKeyPath}");

                var rsa = RSA.Create();
                var pemContent = File.ReadAllText(privateKeyPath);
                rsa.ImportFromPem(pemContent);
                var credentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

                var roles = await _userManager.GetRolesAsync(user);
                if (roles == null || roles.Count == 0)
                    _logger.LogWarning("No roles found for user: {UserName}", user.UserName);

                var claims = new List<Claim>
                {
                    new Claim("sub", user.Id),
                    new Claim("email", user.Email ?? string.Empty)
                };

                if (roles != null)
                {
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim("role", role));
                        _logger.LogInformation("Added role claim: {Role}", role);
                    }
                }


                var expires = DateTime.UtcNow.AddMinutes(
                    double.TryParse(jwtSettings["AccessTokenExpirationMinutes"], out var minutes) ? minutes : 30);

                var token = new JwtSecurityToken(
                    issuer: jwtSettings["Issuer"],
                    audience: jwtSettings["Audience"],
                    claims: claims,
                    expires: expires,
                    signingCredentials: credentials
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                _logger.LogInformation("JWT generated for user {UserName}, expires {Expiry}", user.UserName, expires);

                return (tokenString, expires);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT for user {UserName}", user?.UserName);
                throw;
            }
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ipAddress))
                    ipAddress = "Unknown";

                var randomBytes = RandomNumberGenerator.GetBytes(64);
                var expires = DateTime.UtcNow.AddDays(
                    int.TryParse(_configuration["JwtSettings:RefreshTokenExpirationDays"], out var days) ? days : 7);

                var refreshToken = new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = expires,
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };

                _logger.LogInformation("Refresh token generated from IP {IpAddress}, expires {Expiry}", ipAddress, expires);
                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating refresh token from IP {IpAddress}", ipAddress);
                throw;
            }
        }
    }
}