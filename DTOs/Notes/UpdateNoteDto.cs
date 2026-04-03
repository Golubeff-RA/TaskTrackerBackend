// DTOs/Notes/UpdateNoteDto.cs
using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Notes
{
    public class UpdateNoteDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Content { get; set; }
    }
}