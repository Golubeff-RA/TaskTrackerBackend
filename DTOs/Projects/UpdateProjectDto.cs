using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Projects
{
    public class UpdateProjectDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string ProjectName { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}