using System;

namespace YourApp.DTOs.Contacts
{
    /// <summary>
    /// DTO для ответа с данными контакта
    /// </summary>
    public class ContactResponseDto
    {
        /// <summary>
        /// UUID контакта
        /// </summary>
        /// <example>123e4567-e89b-12d3-a456-426614174000</example>
        public Guid ContactUuid { get; set; }

        /// <summary>
        /// UUID пользователя-владельца
        /// </summary>
        public Guid UserUuid { get; set; }

        /// <summary>
        /// Имя контакта
        /// </summary>
        /// <example>Иван Петров</example>
        public string Name { get; set; }

        /// <summary>
        /// Номер телефона контакта
        /// </summary>
        /// <example>+7 (999) 123-45-67</example>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Email адрес контакта
        /// </summary>
        /// <example>ivan.petrov@example.com</example>
        public string? Email { get; set; }

        /// <summary>
        /// Комментарий к контакту
        /// </summary>
        /// <example>Коллега из отдела разработки</example>
        public string? Comment { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        /// <example>2024-01-15T10:30:00Z</example>
        public long CreatedAt { get; set; }

        /// <summary>
        /// Дата удаления (мягкое удаление)
        /// </summary>
        public long? DeletedAt { get; set; }
    }
}