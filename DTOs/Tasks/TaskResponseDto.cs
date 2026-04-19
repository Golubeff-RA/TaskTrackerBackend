using YourApp.Enums;

namespace YourApp.DTOs.Tasks
{
    public class TaskResponseDto
    {
        public Guid TaskUuid { get; set; }
        public Guid ProjectUuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskaStatus Status { get; set; }
        public int Wave { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? BlockedUntil { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}