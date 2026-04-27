using System.ComponentModel.DataAnnotations;
using YourApp.Enums;

namespace YourApp.DTOs.Projects
{
    public class CreateProjectDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(255)]
        public string ProjectName { get; set; }

        public string Description { get; set; } = string.Empty;

        public ProjectStatus? Status { get; set; } = ProjectStatus.CREATED;
    }
}