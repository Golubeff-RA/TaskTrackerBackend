using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.DTOs.Tasks;
using YourApp.Enums;
using YourApp.Extensions;
using YourApp.Models;
using YourApp.Services.Interfaces;

namespace YourApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TaskService> _logger;

        public TaskService(ApplicationDbContext context, ILogger<TaskService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TaskResponseDto>> GetTasksByProjectAsync(Guid userId, Guid projectId)
        {
            // Проверяем, что проект принадлежит пользователю
            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);

            if (!projectExists)
                throw new KeyNotFoundException($"Project with ID {projectId} not found");

            var tasks = await _context.Tasks
                .Where(t => t.ProjectUuid == projectId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return tasks.Select(Map).ToList();
        }

        public async Task<List<TaskResponseDto>> GetAllTasksAsync(Guid userId)
        {
            var tasks = await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.UserUuid == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            return tasks.Select(Map).ToList();
        }

        public async Task<TaskResponseDto> GetTaskByIdAsync(Guid userId, Guid taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskUuid == taskId && t.Project.UserUuid == userId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            return Map(task);
        }

        public async Task<TaskResponseDto> CreateTaskAsync(Guid userId, Guid projectId, CreateTaskDto dto)
        {
            var projectExists = await _context.Projects
                .AnyAsync(p => p.ProjectUuid == projectId && p.UserUuid == userId);

            if (!projectExists)
                throw new KeyNotFoundException($"Project with ID {projectId} not found");

            var task = new Taska
            {
                ProjectUuid = projectId,
                Title = dto.Title,
                Description = dto.Description,
                Wave = dto.Wave,
                Status = dto.Status ?? TaskaStatus.CREATED,
                BlockedUntil = dto.BlockedUntilMs.HasValue 
                    ? dto.BlockedUntilMs.Value.FromUnixMs() 
                    : null,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Task {Id} created in project {ProjectId}", task.TaskUuid, projectId);
            return Map(task);
        }

        public async Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskUuid == taskId && t.Project.UserUuid == userId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            if (dto.Title != null) task.Title = dto.Title;
            if (dto.Description != null) task.Description = dto.Description;
            if (dto.Wave.HasValue) task.Wave = dto.Wave.Value; 
            if (dto.Status.HasValue) task.Status = dto.Status.Value;
            if (dto.BlockedUntilMs.HasValue) task.BlockedUntil = dto.BlockedUntilMs.Value.FromUnixMs();

            await _context.SaveChangesAsync();
            return Map(task);
        }

        public async Task<TaskResponseDto?> CloseTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskUuid == taskId && t.Project.UserUuid == userId);
            if (task == null) return null;

            task.Status = TaskaStatus.CLOSED;
            task.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Map(task);
        }

        public async Task<TaskResponseDto?> BlockTaskAsync(Guid userId, Guid taskId, BlockTaskDto dto)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskUuid == taskId && t.Project.UserUuid == userId);
            if (task == null) return null;

            task.Status = TaskaStatus.BLOCKED;
            task.BlockedUntil = dto.BlockedUntilMs.HasValue
                ? dto.BlockedUntilMs.Value.FromUnixMs()
                : null;

            await _context.SaveChangesAsync();

            return Map(task);
        }

        public async Task<TaskResponseDto?> UnblockTaskAsync(Guid userId, Guid taskId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.TaskUuid == taskId && t.Project.UserUuid == userId);
            if (task == null) return null;

            task.Status = TaskaStatus.CREATED;
            task.BlockedUntil = null;
            await _context.SaveChangesAsync();

            return Map(task);
        }

        public async Task<TaskResponseDto?> GetRandomTaskAsync(Guid userId)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .Where(t => t.Project.UserUuid == userId && t.Status == TaskaStatus.CREATED)
                .OrderBy(_ => Guid.NewGuid())
                .FirstOrDefaultAsync();

            return task == null ? null : Map(task);
        }

        private static TaskResponseDto Map(Taska t) => new()
        {
            TaskUuid = t.TaskUuid,
            ProjectUuid = t.ProjectUuid,
            Title = t.Title,
            Description = t.Description,
            Status = (int)t.Status,
            Wave = t.Wave,
            CreatedAt = t.CreatedAt.ToUnixMs(),
            BlockedUntil = t.BlockedUntil.ToUnixMs() ?? 0,
            CompletedAt = t.CompletedAt.ToUnixMs(),
            DeletedAt = t.DeletedAt.ToUnixMs()
        };
    }
}
