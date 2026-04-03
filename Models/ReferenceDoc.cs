// Models/ReferenceDoc.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourApp.Models
{
    [Table("reference_docs")]
    public class ReferenceDoc
    {
        [Key]
        [Column("doc_uuid")]
        public Guid DocUuid { get; set; } = Guid.NewGuid();

        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Column("title")]
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserUuid))]
        public virtual User User { get; set; }
    }
}