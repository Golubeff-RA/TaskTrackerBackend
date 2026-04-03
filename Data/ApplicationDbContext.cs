// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using YourApp.Models;
using YourApp.Enums;

namespace YourApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Taska> Tasks { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<ReferenceDoc> ReferenceDocs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<User>()
                .Property(u => u.RefreshToken)
                .IsRequired(false);

            modelBuilder.Entity<User>()
                .Property(u => u.RefreshTokenExpiresAt)
                .IsRequired(false);

            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>()
                .HasDefaultValue(ProjectStatus.CREATED);

            modelBuilder.Entity<Taska>()
                .Property(t => t.Status)
                .HasConversion<string>()
                .HasDefaultValue(TaskaStatus.CREATED);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.UserUuid);

            modelBuilder.Entity<Taska>()
                .HasIndex(t => t.ProjectUuid);

            modelBuilder.Entity<Mark>()
                .HasIndex(m => m.ProjectUuid);

            modelBuilder.Entity<Note>()
                .HasIndex(n => n.UserUuid);

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.UserUuid);

            modelBuilder.Entity<ReferenceDoc>()
                .HasIndex(r => r.UserUuid);

            modelBuilder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            modelBuilder.Entity<Project>().HasQueryFilter(p => p.DeletedAt == null);
            modelBuilder.Entity<Taska>().HasQueryFilter(t => t.DeletedAt == null);
            modelBuilder.Entity<Mark>().HasQueryFilter(m => m.DeletedAt == null);
            modelBuilder.Entity<Note>().HasQueryFilter(n => n.DeletedAt == null);
            modelBuilder.Entity<Contact>().HasQueryFilter(c => c.DeletedAt == null);
            modelBuilder.Entity<ReferenceDoc>().HasQueryFilter(r => r.DeletedAt == null);
        }
    }
}