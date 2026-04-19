using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.Marks
{
    public class MarkResponseDto
    {
        public Guid MarkUuid { get; set; }
        public Guid ProjectUuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}