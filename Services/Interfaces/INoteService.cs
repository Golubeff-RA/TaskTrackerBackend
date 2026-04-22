using YourApp.DTOs.Notes;

namespace YourApp.Services.Interfaces
{
    public interface INoteService
    {
        Task<NoteResponseDto> CreateNoteAsync(Guid userId, CreateNoteDto dto);
        Task<NoteResponseDto> GetNoteByIdAsync(Guid userId, Guid noteId);
        Task<List<NoteResponseDto>> GetAllNotesAsync(Guid userId);
        Task<NoteResponseDto?> DeleteNoteAsync(Guid userId, Guid noteId);
    }
}