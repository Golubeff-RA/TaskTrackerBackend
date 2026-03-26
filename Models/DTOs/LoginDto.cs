using System.ComponentModel.DataAnnotations;

namespace AuthApi.Models.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Имя пользователя или email обязателен")]
        public string UsernameOrEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; } = string.Empty;
    }
}