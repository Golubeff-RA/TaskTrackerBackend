// Services/NoteService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.DTOs.Notes;
using YourApp.Models;
using YourApp.Services.Interfaces;

namespace YourApp.Services
{
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<NoteService> _logger;

        public NoteService(ApplicationDbContext context, ILogger<NoteService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<NoteResponseDto> CreateNoteAsync(Guid userId, CreateNoteDto createNoteDto)
        {
            _logger.LogInformation($"Creating note for user: {userId}");

            var note = new Note
            {
                UserUuid = userId,
                Title = createNoteDto.Title,
                Content = createNoteDto.Content,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notes.AddAsync(note);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Note created with ID: {note.NoteUuid} for user: {userId}");

            return MapToResponseDto(note);
        }

        public async Task<NoteResponseDto> GetNoteByIdAsync(Guid userId, Guid noteId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteUuid == noteId && n.UserUuid == userId);

            if (note == null)
            {
                throw new KeyNotFoundException("Заметка не найдена");
            }

            return MapToResponseDto(note);
        }

        public async Task<List<NoteResponseDto>> GetAllNotesAsync(Guid userId)
        {
            _logger.LogInformation($"Getting all notes for user: {userId}");

            var notes = await _context.Notes
                .Where(n => n.UserUuid == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            _logger.LogInformation($"Found {notes.Count} notes for user: {userId}");

            // Логируем все найденные заметки
            foreach (var note in notes)
            {
                _logger.LogInformation($"Note: {note.NoteUuid} - User: {note.UserUuid}");
            }

            return notes.Select(MapToResponseDto).ToList();
        }

        public async Task<NoteResponseDto> UpdateNoteAsync(Guid userId, Guid noteId, UpdateNoteDto updateNoteDto)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteUuid == noteId && n.UserUuid == userId);

            if (note == null)
            {
                throw new KeyNotFoundException("Заметка не найдена");
            }

            note.Title = updateNoteDto.Title;
            note.Content = updateNoteDto.Content;

            await _context.SaveChangesAsync();

            return MapToResponseDto(note);
        }

        public async Task<bool> DeleteNoteAsync(Guid userId, Guid noteId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteUuid == noteId && n.UserUuid == userId);

            if (note == null)
            {
                return false;
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteNoteAsync(Guid userId, Guid noteId)
        {
            var note = await _context.Notes
                .FirstOrDefaultAsync(n => n.NoteUuid == noteId && n.UserUuid == userId);

            if (note == null)
            {
                return false;
            }

            note.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private NoteResponseDto MapToResponseDto(Note note)
        {
            return new NoteResponseDto
            {
                NoteUuid = note.NoteUuid,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                DeletedAt = note.DeletedAt
            };
        }
    }
}