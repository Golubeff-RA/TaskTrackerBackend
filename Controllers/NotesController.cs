// Controllers/NotesController.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YourApp.DTOs.Notes;
using YourApp.Services.Interfaces;
using YourApp.Extensions;

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

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto createNoteDto)
        {
            var userId = User.GetUserId();
            var note = await _noteService.CreateNoteAsync(userId, createNoteDto);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.NoteUuid }, note);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            var userId = User.GetUserId();
            var notes = await _noteService.GetAllNotesAsync(userId);
            return Ok(notes);
        }

        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
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

        [HttpDelete("{id}/soft")]
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