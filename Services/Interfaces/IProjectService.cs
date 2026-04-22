using YourApp.DTOs.Projects;

namespace YourApp.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectResponseDto>> GetAllProjectsAsync(Guid userId);
        Task<ProjectResponseDto> GetProjectByIdAsync(Guid userId, Guid projectId);
        Task<ProjectResponseDto> CreateProjectAsync(Guid userId, CreateProjectDto dto);
        Task<ProjectResponseDto> UpdateProjectAsync(Guid userId, Guid projectId, UpdateProjectDto dto);
        Task<ProjectResponseDto?> CloseProjectAsync(Guid userId, Guid projectId);
    }
}