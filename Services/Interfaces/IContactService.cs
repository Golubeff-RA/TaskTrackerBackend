// Services/Interfaces/IContactService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourApp.DTOs.Contacts;

namespace YourApp.Services.Interfaces
{
    /// <summary>
    /// Сервис для работы с контактами
    /// </summary>
    public interface IContactService
    {
        /// <summary>
        /// Создание нового контакта
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="createContactDto">Данные для создания контакта</param>
        /// <returns>Созданный контакт</returns>
        Task<ContactResponseDto> CreateContactAsync(Guid userId, CreateContactDto createContactDto);

        /// <summary>
        /// Получение контакта по ID
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="contactId">ID контакта</param>
        /// <returns>Контакт</returns>
        Task<ContactResponseDto> GetContactByIdAsync(Guid userId, Guid contactId);

        /// <summary>
        /// Получение всех контактов пользователя
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <returns>Список контактов</returns>
        Task<List<ContactResponseDto>> GetAllContactsAsync(Guid userId);

        /// <summary>
        /// Обновление контакта
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="contactId">ID контакта</param>
        /// <param name="updateContactDto">Обновленные данные контакта</param>
        /// <returns>Обновленный контакт</returns>
        Task<ContactResponseDto> UpdateContactAsync(Guid userId, Guid contactId, UpdateContactDto updateContactDto);

        /// <summary>
        /// Полное удаление контакта из базы данных
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="contactId">ID контакта</param>
        /// <returns>Результат удаления</returns>
        Task<bool> DeleteContactAsync(Guid userId, Guid contactId);

        /// <summary>
        /// Мягкое удаление контакта (установка DeletedAt)
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="contactId">ID контакта</param>
        /// <returns>Результат удаления</returns>
        Task<bool> SoftDeleteContactAsync(Guid userId, Guid contactId);

        /// <summary>
        /// Поиск контактов по имени, телефону, email или комментарию
        /// </summary>
        /// <param name="userId">ID пользователя</param>
        /// <param name="searchTerm">Поисковый запрос</param>
        /// <returns>Список найденных контактов</returns>
        Task<List<ContactResponseDto>> SearchContactsAsync(Guid userId, string searchTerm);
    }
}