// DTOs/Notes/NoteResponseDto.cs
using System;

namespace YourApp.DTOs.Notes
{
    public class NoteResponseDto
    {
        public Guid NoteUuid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}