using YourApp.DTOs.ReferenceDocs;

namespace YourApp.Services.Interfaces
{
    public interface IReferenceDocService
    {
        Task<List<ReferenceDocResponseDto>> GetAllAsync(Guid userId);
        Task<ReferenceDocResponseDto> GetByIdAsync(Guid userId, Guid docId);
        Task<ReferenceDocResponseDto> CreateAsync(Guid userId, CreateReferenceDocDto dto);
        Task<ReferenceDocResponseDto> UpdateAsync(Guid userId, Guid docId, UpdateReferenceDocDto dto);
        Task<bool> DeleteAsync(Guid userId, Guid docId);
    }
}