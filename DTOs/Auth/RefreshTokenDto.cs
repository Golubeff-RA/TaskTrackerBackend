// DTOs/Auth/RefreshTokenDto.cs
using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Auth
{
    public class RefreshTokenDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}