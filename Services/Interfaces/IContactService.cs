using YourApp.DTOs.Contacts;

namespace YourApp.Services.Interfaces
{
    public interface IContactService
    {
        Task<ContactResponseDto> CreateContactAsync(Guid userId, CreateContactDto dto);
        Task<ContactResponseDto> GetContactByIdAsync(Guid userId, Guid contactId);
        Task<List<ContactResponseDto>> GetAllContactsAsync(Guid userId);
        Task<ContactResponseDto> UpdateContactAsync(Guid userId, Guid contactId, UpdateContactDto dto);
        Task<ContactResponseDto?> DeleteContactAsync(Guid userId, Guid contactId);
    }
}