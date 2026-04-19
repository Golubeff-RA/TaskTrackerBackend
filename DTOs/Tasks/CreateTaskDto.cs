using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Tasks
{
    public class CreateTaskDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public int Wave { get; set; } = 0;
    }
}
