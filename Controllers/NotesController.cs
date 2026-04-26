using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Notes;
using YourApp.Extensions;
using YourApp.Services.Interfaces;

namespace YourApp.Controllers
{
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
        /// Создать заметку
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(NoteResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
        {
            var userId = User.GetUserId();
            var note = await _noteService.CreateNoteAsync(userId, dto);
            return Ok(note);
        }

        /// <summary>
        /// Список всех заметок пользователя
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(List<NoteResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();
            var notes = await _noteService.GetAllNotesAsync(userId);
            return Ok(notes);
        }

        /// <summary>
        /// Получить одну заметку
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NoteResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var userId = User.GetUserId();
                var note = await _noteService.GetNoteByIdAsync(userId, id);
                return Ok(note);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Удалить заметку (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(NoteResponseDto), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var note = await _noteService.DeleteNoteAsync(userId, id);

            if (note == null)
                return NotFound(new { error = "Note not found" });

            return Ok(note);
        }
    }
}