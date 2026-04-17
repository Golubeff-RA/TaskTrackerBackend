using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Contacts;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
    /// <summary>
    /// Контроллер для управления контактами пользователя
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Создание нового контакта
        /// </summary>
        /// <param name="createContactDto">Данные для создания контакта</param>
        /// <returns>Созданный контакт</returns>
        /// <response code="201">Контакт успешно создан</response>
        /// <response code="400">Ошибка валидации</response>
        /// <response code="401">Не авторизован</response>
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateContact([FromBody] CreateContactDto createContactDto)
        {
            var userId = User.GetUserId();
            var contact = await _contactService.CreateContactAsync(userId, createContactDto);
            return CreatedAtAction(nameof(GetContactById), new { id = contact.ContactUuid }, contact);
        }

        /// <summary>
        /// Получение всех контактов текущего пользователя
        /// </summary>
        /// <returns>Список контактов</returns>
        /// <response code="200">Успешное получение списка контактов</response>
        /// <response code="401">Не авторизован</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContactResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAllContacts()
        {
            var userId = User.GetUserId();
            var contacts = await _contactService.GetAllContactsAsync(userId);
            return Ok(contacts);
        }

        /// <summary>
        /// Получение контакта по ID
        /// </summary>
        /// <param name="id">UUID контакта</param>
        /// <returns>Контакт</returns>
        /// <response code="200">Контакт найден</response>
        /// <response code="404">Контакт не найден</response>
        /// <response code="401">Не авторизован</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetContactById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var contact = await _contactService.GetContactByIdAsync(userId, id);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Обновление контакта
        /// </summary>
        /// <param name="id">UUID контакта</param>
        /// <param name="updateContactDto">Обновленные данные контакта</param>
        /// <returns>Обновленный контакт</returns>
        /// <response code="200">Контакт успешно обновлен</response>
        /// <response code="404">Контакт не найден</response>
        /// <response code="400">Ошибка валидации</response>
        /// <response code="401">Не авторизован</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateContact(Guid id, [FromBody] UpdateContactDto updateContactDto)
        {
            try
            {
                var userId = User.GetUserId();
                var contact = await _contactService.UpdateContactAsync(userId, id, updateContactDto);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Полное удаление контакта (из базы данных)
        /// </summary>
        /// <param name="id">UUID контакта</param>
        /// <response code="204">Контакт успешно удален</response>
        /// <response code="404">Контакт не найден</response>
        /// <response code="401">Не авторизован</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _contactService.DeleteContactAsync(userId, id);

            if (!result)
            {
                return NotFound(new { message = "Контакт не найден" });
            }

            return NoContent();
        }

        /// <summary>
        /// Мягкое удаление контакта (установка deleted_at)
        /// </summary>
        /// <param name="id">UUID контакта</param>
        /// <response code="200">Контакт успешно удален</response>
        /// <response code="404">Контакт не найден</response>
        /// <response code="401">Не авторизован</response>
        [HttpDelete("{id}/soft")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SoftDeleteContact(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _contactService.SoftDeleteContactAsync(userId, id);

            if (!result)
            {
                return NotFound(new { message = "Контакт не найден" });
            }

            return Ok(new { message = "Контакт удален" });
        }

        /// <summary>
        /// Поиск контактов по имени или комментарию
        /// </summary>
        /// <param name="searchTerm">Поисковый запрос</param>
        /// <returns>Список найденных контактов</returns>
        /// <response code="200">Результаты поиска</response>
        /// <response code="401">Не авторизован</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<ContactResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SearchContacts([FromQuery] string searchTerm)
        {
            var userId = User.GetUserId();
            var contacts = await _contactService.SearchContactsAsync(userId, searchTerm);
            return Ok(contacts);
        }
    }
}