using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Contacts
{
    /// <summary>
    /// DTO для обновления контакта
    /// </summary>
    public class UpdateContactDto
    {
        /// <summary>
        /// Имя контакта
        /// </summary>
        /// <example>Иван Петров (обновлено)</example>
        [Required(ErrorMessage = "Имя контакта обязательно")]
        [MinLength(1, ErrorMessage = "Имя не может быть пустым")]
        [MaxLength(255, ErrorMessage = "Имя не может быть длиннее 255 символов")]
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона контакта
        /// </summary>
        /// <example>+7 (999) 123-45-67</example>
        [MaxLength(20, ErrorMessage = "Номер телефона не может быть длиннее 20 символов")]
        [Phone(ErrorMessage = "Неверный формат номера телефона")]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email адрес контакта
        /// </summary>
        /// <example>ivan.petrov@example.com</example>
        [MaxLength(255, ErrorMessage = "Email не может быть длиннее 255 символов")]
        [EmailAddress(ErrorMessage = "Неверный формат email адреса")]
        public string? Email { get; set; }

        /// <summary>
        /// Комментарий к контакту
        /// </summary>
        /// <example>Ведущий разработчик</example>
        [MaxLength(1000, ErrorMessage = "Комментарий не может быть длиннее 1000 символов")]
        public string? Comment { get; set; }
    }
}