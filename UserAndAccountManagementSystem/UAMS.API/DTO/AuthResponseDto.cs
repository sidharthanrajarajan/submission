namespace UAMS.API.DTO
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpires { get; set; }
    }
}