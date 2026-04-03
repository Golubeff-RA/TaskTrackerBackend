// DTOs/Auth/RegisterDto.cs
using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}