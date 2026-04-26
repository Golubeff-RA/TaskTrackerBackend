// DTOs/Notes/NoteResponseDto.cs
using System;

namespace YourApp.DTOs.Notes
{
    public class NoteResponseDto
    {
        public Guid NoteUuid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}