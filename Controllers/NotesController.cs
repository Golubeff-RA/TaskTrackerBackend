using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Notes;
using YourApp.Services.Interfaces;
using YourApp.Extensions;

namespace YourApp.Controllers
{
    /// <summary>
    /// Контроллер для управления заметками пользователя
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// Создание новой заметки
        /// </summary>
        /// <param name="createNoteDto">Данные для создания заметки</param>
        /// <returns>Созданная заметка</returns>
        /// <response code="201">Заметка успешно создана</response>
        /// <response code="400">Ошибка валидации</response>
        /// <response code="401">Не авторизован</response>
        [HttpPost]
        [ProducesResponseType(typeof(NoteResponseDto), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto createNoteDto)
        {
            var userId = User.GetUserId();
            var note = await _noteService.CreateNoteAsync(userId, createNoteDto);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.NoteUuid }, note);
        }

        /// <summary>
        /// Получение всех заметок текущего пользователя
        /// </summary>
        /// <returns>Список заметок</returns>
        /// <response code="200">Успешное получение списка заметок</response>
        /// <response code="401">Не авторизован</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<NoteResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = User.GetUserId();
            var notes = await _noteService.GetAllNotesAsync(userId);
            return Ok(notes);
        }

        /// <summary>
        /// Получение заметки по ID
        /// </summary>
        /// <param name="id">UUID заметки</param>
        /// <returns>Заметка</returns>
        /// <response code="200">Заметка найдена</response>
        /// <response code="404">Заметка не найдена</response>
        /// <response code="401">Не авторизован</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NoteResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetNoteById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var note = await _noteService.GetNoteByIdAsync(userId, id);
                return Ok(note);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Обновление заметки
        /// </summary>
        /// <param name="id">UUID заметки</param>
        /// <param name="updateNoteDto">Обновленные данные заметки</param>
        /// <returns>Обновленная заметка</returns>
        /// <response code="200">Заметка успешно обновлена</response>
        /// <response code="404">Заметка не найдена</response>
        /// <response code="400">Ошибка валидации</response>
        /// <response code="401">Не авторизован</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(NoteResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UpdateNote(Guid id, [FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                var userId = User.GetUserId();
                var note = await _noteService.UpdateNoteAsync(userId, id, updateNoteDto);
                return Ok(note);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Полное удаление заметки (из базы данных)
        /// </summary>
        /// <param name="id">UUID заметки</param>
        /// <response code="204">Заметка успешно удалена</response>
        /// <response code="404">Заметка не найдена</response>
        /// <response code="401">Не авторизован</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> DeleteNote(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _noteService.DeleteNoteAsync(userId, id);
            
            if (!result)
            {
                return NotFound(new { message = "Заметка не найдена" });
            }
            
            return NoContent();
        }

        /// <summary>
        /// Мягкое удаление заметки (установка deleted_at)
        /// </summary>
        /// <param name="id">UUID заметки</param>
        /// <response code="200">Заметка успешно удалена</response>
        /// <response code="404">Заметка не найдена</response>
        /// <response code="401">Не авторизован</response>
        [HttpDelete("{id}/soft")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> SoftDeleteNote(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _noteService.SoftDeleteNoteAsync(userId, id);
            
            if (!result)
            {
                return NotFound(new { message = "Заметка не найдена" });
            }
            
            return Ok(new { message = "Заметка удалена" });
        }
    }
}