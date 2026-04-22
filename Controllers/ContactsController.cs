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
        /// Список контактов пользователя
        /// </summary>
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
        /// Получить контакт по ID
        /// </summary>
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
        /// Создать контакт
        /// </summary>
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
        /// Изменить контакт
        /// </summary>
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
        /// Удалить контакт
        /// </summary>
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