using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Contacts;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
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
        /// Получение всех контактов текущего пользователя
        /// </summary>
        /// <returns>Список контактов</returns>
        /// <response code="200">Успешное получение списка контактов</response>
        /// <response code="401">Не авторизован</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<ContactResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
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
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var contact = await _contactService.GetContactByIdAsync(userId, id);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
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
        [ProducesResponseType(typeof(ContactResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CreateContactDto dto)
        {
            var userId = User.GetUserId();
            var contact = await _contactService.CreateContactAsync(userId, dto);
            return Ok(contact);
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
        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(ContactResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateContactDto dto)
        {
            try
            {
                var userId = User.GetUserId();
                var contact = await _contactService.UpdateContactAsync(userId, id, dto);
                return Ok(contact);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
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
        [ProducesResponseType(typeof(ContactResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var contact = await _contactService.DeleteContactAsync(userId, id);

            if (contact == null)
                return NotFound(new { error = "Contact not found" });

            return Ok(contact);
        }
    }
}