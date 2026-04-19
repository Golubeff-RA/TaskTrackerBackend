using YourApp.DTOs.Marks;

namespace YourApp.Services.Interfaces
{
    public interface IMarkService
    {
        Task<List<MarkResponseDto>> GetMarksByProjectAsync(Guid userId, Guid projectId);
        Task<MarkResponseDto> CreateMarkAsync(Guid userId, Guid projectId, CreateMarkDto dto);
        Task<MarkResponseDto> UpdateMarkAsync(Guid userId, Guid markId, UpdateMarkDto dto);
        Task<bool> DeleteMarkAsync(Guid userId, Guid markId);
    }
}
