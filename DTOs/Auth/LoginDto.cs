// DTOs/Auth/LoginDto.cs
using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}