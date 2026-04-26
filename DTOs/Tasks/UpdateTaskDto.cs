using System.ComponentModel.DataAnnotations;
using YourApp.Enums;

namespace YourApp.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        [MinLength(1)]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public int? Wave { get; set; }

        public TaskaStatus? Status { get; set; }

        public long? BlockedUntilMs { get; set; }
    }
}
