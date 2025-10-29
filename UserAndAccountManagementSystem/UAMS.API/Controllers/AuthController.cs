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
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Unauthorized("Invalid username or password.");

                var (token, expires) = await _jwtTokenService.GenerateAccessTokenAsync(user);

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error in AuthController.Login: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred while processing your request.");
            }
        }

    }
}
