// Models/Project.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourApp.Enums;

namespace YourApp.Models
{
    [Table("projects")]
    public class Project
    {
        [Key]
        [Column("project_uuid")]
        public Guid ProjectUuid { get; set; } = Guid.NewGuid();

        [Column("user_uuid")]
        public Guid UserUuid { get; set; }

        [Column("project_name")]
        [Required]
        [MaxLength(255)]
        public string ProjectName { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("status")]
        public ProjectStatus Status { get; set; } = ProjectStatus.CREATED;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("closed_at")]
        public DateTime? ClosedAt { get; set; }

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(UserUuid))]
        public virtual User User { get; set; }
        
        public virtual ICollection<Taska> Tasks { get; set; }
        public virtual ICollection<Mark> Marks { get; set; }
    }
}