// Services/Interfaces/INoteService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YourApp.DTOs.Notes;

namespace YourApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteResponseDto> CreateNoteAsync(Guid userId, CreateNoteDto createNoteDto);
        Task<NoteResponseDto> GetNoteByIdAsync(Guid userId, Guid noteId);
        Task<List<NoteResponseDto>> GetAllNotesAsync(Guid userId);
        Task<NoteResponseDto> UpdateNoteAsync(Guid userId, Guid noteId, UpdateNoteDto updateNoteDto);
        Task<bool> DeleteNoteAsync(Guid userId, Guid noteId);
        Task<bool> SoftDeleteNoteAsync(Guid userId, Guid noteId);
    }
}