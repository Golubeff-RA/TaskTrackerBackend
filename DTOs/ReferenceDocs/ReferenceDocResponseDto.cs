using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.ReferenceDocs
{
    public class ReferenceDocResponseDto
    {
        public Guid DocUuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}