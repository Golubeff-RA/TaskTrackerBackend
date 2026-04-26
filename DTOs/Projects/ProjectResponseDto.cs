using YourApp.Enums;

namespace YourApp.DTOs.Projects
{
    public class ProjectResponseDto
    {
        public Guid ProjectUuid { get; set; }
        public Guid UserUuid { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long CreatedAt { get; set; }
        public long? ClosedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}