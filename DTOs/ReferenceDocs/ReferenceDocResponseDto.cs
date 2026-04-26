using System.ComponentModel.DataAnnotations;

namespace YourApp.DTOs.ReferenceDocs
{
    public class ReferenceDocResponseDto
    {
        public Guid DocUuid { get; set; }
        public Guid UserUuid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
        public long? DeletedAt { get; set; }
    }
}