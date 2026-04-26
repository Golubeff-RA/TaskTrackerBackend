using YourApp.Enums;

namespace YourApp.DTOs.Tasks
{
    public class TaskResponseDto
    {
        public Guid TaskUuid { get; set; }
        public Guid ProjectUuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Wave { get; set; }
        public long CreatedAt { get; set; }
        public long? BlockedUntil { get; set; }
        public long? CompletedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}