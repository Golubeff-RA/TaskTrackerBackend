// Models/Task.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourApp.Enums;

namespace YourApp.Models
{
    [Table("tasks")]
    public class Taska
    {
        [Key]
        [Column("task_uuid")]
        public Guid TaskUuid { get; set; } = Guid.NewGuid();

        [Column("project_uuid")]
        public Guid ProjectUuid { get; set; }

        [Column("title")]
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("status")]
        public TaskaStatus Status { get; set; } = TaskaStatus.CREATED;

        [Column("blocked_until")]
        public DateTime? BlockedUntil { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("completed_at")]
        public DateTime? CompletedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ProjectUuid))]
        public virtual Project Project { get; set; }
    }
}