// Models/Mark.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourApp.Models
{
    [Table("marks")]
    public class Mark
    {
        [Key]
        [Column("mark_uuid")]
        public Guid MarkUuid { get; set; } = Guid.NewGuid();

        [Column("project_uuid")]
        public Guid ProjectUuid { get; set; }

        [Column("title")]
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ProjectUuid))]
        public virtual Project Project { get; set; }
    }
}