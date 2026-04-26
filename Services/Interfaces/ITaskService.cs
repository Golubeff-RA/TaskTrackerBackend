using YourApp.DTOs.Tasks;

namespace YourApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetTasksByProjectAsync(Guid userId, Guid projectId);
        Task<TaskResponseDto> GetTaskByIdAsync(Guid userId, Guid taskId);
        Task<TaskResponseDto> CreateTaskAsync(Guid userId, Guid projectId, CreateTaskDto dto);
        Task<TaskResponseDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto);
        Task<TaskResponseDto?> CloseTaskAsync(Guid userId, Guid taskId);
        Task<TaskResponseDto?> BlockTaskAsync(Guid userId, Guid taskId, BlockTaskDto dto);
        Task<TaskResponseDto?> GetRandomTaskAsync(Guid userId);
    }
}