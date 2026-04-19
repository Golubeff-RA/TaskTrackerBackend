using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Marks
{
    public class CreateMarkDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}