using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.ReferenceDocs
{
    public class CreateReferenceDocDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}