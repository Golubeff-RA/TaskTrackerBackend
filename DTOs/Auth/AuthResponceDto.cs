// DTOs/Auth/AuthResponseDto.cs
namespace YourApp.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public Guid UserId { get; set; }
        public long AccessTokenExpiresAt { get; set; }
    }
}