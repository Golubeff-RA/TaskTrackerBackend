using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.DTOs.Projects;
using YourApp.Enums;
using YourApp.Models;
using YourApp.Services.Interfaces;

namespace YourApp.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProjectService> _logger;

        public ProjectService(ApplicationDbContext context, ILogger<ProjectService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ProjectResponseDto>> GetAllProjectsAsync(Guid userId)
        {
            var projects = await _context.Projects
                .Where(p => p.UserUuid == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return projects.Select(Map).ToList();
        }

        public async Task<ProjectResponseDto> GetProjectByIdAsync(Guid userId, Guid projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found");

            return Map(project);
        }

        public async Task<ProjectResponseDto> CreateProjectAsync(Guid userId, CreateProjectDto dto)
        {
            var project = new Project
            {
                UserUuid = userId,
                ProjectName = dto.ProjectName,
                Description = dto.Description,
                Status = ProjectStatus.CREATED,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Project {Id} created for user {UserId}", project.ProjectUuid, userId);
            return Map(project);
        }

        public async Task<ProjectResponseDto> UpdateProjectAsync(Guid userId, Guid projectId, UpdateProjectDto dto)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);

            if (project == null)
                throw new KeyNotFoundException($"Project with ID {projectId} not found");

            if (dto.ProjectName != null) project.ProjectName = dto.ProjectName;
            if (dto.Description != null) project.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Map(project);
        }

        public async Task<ProjectResponseDto?> CloseProjectAsync(Guid userId, Guid projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);
            if (project == null) return null;

            project.Status = ProjectStatus.CLOSED;
            project.ClosedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Map(project);
        }

        private static ProjectResponseDto Map(Project p) => new()
        {
            ProjectUuid = p.ProjectUuid,
            UserUuid = p.UserUuid,
            ProjectName = p.ProjectName,
            Description = p.Description,
            Status = p.Status,
            CreatedAt = p.CreatedAt,
            ClosedAt = p.ClosedAt,
            DeletedAt = p.DeletedAt
        };
    }
}
