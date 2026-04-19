using YourApp.Enums;

namespace YourApp.DTOs.Projects
{
    public class ProjectResponseDto
    {
        public Guid ProjectUuid { get; set; }
        public Guid UserUuid { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}