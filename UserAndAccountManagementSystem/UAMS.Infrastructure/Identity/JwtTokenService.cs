//let's load private key , generate a JWT using RS256 and try to create a refresh token
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UAMS.Domain.Entities;

namespace UAMS.Infrastructure.Identity
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string token, DateTime expires) GenerateAccessToken(ApplicationUser user, IList<string> roles)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var privateKeyPath = jwtSettings["PrivateKeyPath"];

            if (string.IsNullOrWhiteSpace(privateKeyPath))
                throw new InvalidOperationException("Private key path is missing or not configured in JwtSettings.");

            if (!File.Exists(privateKeyPath))
                throw new FileNotFoundException($"Private key file not found at path: {privateKeyPath}");

            using var rsa = RSA.Create();
            rsa.ImportFromPem(File.ReadAllText(privateKeyPath));

            var credentials = new SigningCredentials(
                new RsaSecurityKey(rsa),
                SecurityAlgorithms.RsaSha256
            );

            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
        new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Name, user.UserName ?? "")
    };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["AccessTokenExpirationMinutes"]!)),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, token.ValidTo);
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var randomBytes = RandomNumberGenerator.GetBytes(64);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(int.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]!)),
                CreatedByIp = ipAddress
            };
        }
    }
}
