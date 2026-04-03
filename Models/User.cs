// Models/User.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourApp.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_uuid")]
        public Guid UserUuid { get; set; } = Guid.NewGuid();

        [Column("username")]
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Column("email")]
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }

        [Column("password_hash")]
        [Required]
        public string PasswordHash { get; set; }

        [Column("refresh_token")]
        public string? RefreshToken { get; set; }  // <-- Сделайте nullable

        [Column("refresh_token_expires_at")]
        public DateTime? RefreshTokenExpiresAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Note> Notes { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<ReferenceDoc> ReferenceDocs { get; set; }
    }
}