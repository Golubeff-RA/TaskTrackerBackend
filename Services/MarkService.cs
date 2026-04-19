using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.DTOs.Marks;
using YourApp.Models;
using YourApp.Services.Interfaces;

namespace YourApp.Services
{
    public class MarkService : IMarkService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MarkService> _logger;

        public MarkService(ApplicationDbContext context, ILogger<MarkService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<MarkResponseDto>> GetMarksByProjectAsync(Guid userId, Guid projectId)
        {
            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);

            if (!projectExists)
                throw new KeyNotFoundException($"Project with ID {projectId} not found");

            var marks = await _context.Marks
                .Where(m => m.ProjectUuid == projectId)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return marks.Select(Map).ToList();
        }

        public async Task<MarkResponseDto> CreateMarkAsync(Guid userId, Guid projectId, CreateMarkDto dto)
        {
            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);

            if (!projectExists)
                throw new KeyNotFoundException($"Project with ID {projectId} not found");

            var mark = new Mark
            {
                ProjectUuid = projectId,
                Title = dto.Title,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Marks.AddAsync(mark);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Mark {Id} created in project {ProjectId}", mark.MarkUuid, projectId);
            return Map(mark);
        }

        public async Task<MarkResponseDto> UpdateMarkAsync(Guid userId, Guid markId, UpdateMarkDto dto)
        {
            var mark = await _context.Marks
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.MarkUuid == markId && m.Project.UserUuid == userId);

            if (mark == null)
                throw new KeyNotFoundException($"Mark with ID {markId} not found");

            mark.Title = dto.Title;
            mark.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Map(mark);
        }

        public async Task<bool> DeleteMarkAsync(Guid userId, Guid markId)
        {
            var mark = await _context.Marks
                .Include(m => m.Project)
                .FirstOrDefaultAsync(m => m.MarkUuid == markId && m.Project.UserUuid == userId);

            if (mark == null) return false;

            mark.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private static MarkResponseDto Map(Mark m) => new()
        {
            MarkUuid = m.MarkUuid,
            ProjectUuid = m.ProjectUuid,
            Title = m.Title,
            Description = m.Description,
            CreatedAt = m.CreatedAt,
            DeletedAt = m.DeletedAt
        };
    }
}
