using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UAMS.API.DTO;
using UAMS.Infrastructure.Identity;

namespace UAMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(UserManager<ApplicationUser> userManager, JwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid username or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var (token, expires) = _jwtTokenService.GenerateAccessToken(user, roles);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(
                HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            );

            var response = new AuthResponseDto
            {
                Token = token,
                TokenExpires = expires,
                RefreshToken = refreshToken.Token,
                RefreshTokenExpires = refreshToken.Expires
            };

            return Ok(response);
        }
    }
}
