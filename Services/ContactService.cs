// Services/ContactService.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using YourApp.Data;
using YourApp.DTOs.Contacts;
using YourApp.Extensions;
using YourApp.Models;
using YourApp.Services.Interfaces;

namespace YourApp.Services
{
    /// <summary>
    /// Реализация сервиса для работы с контактами
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactService> _logger;

        public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ContactResponseDto> CreateContactAsync(Guid userId, CreateContactDto createContactDto)
        {
            _logger.LogInformation($"Creating contact for user: {userId}");

            var contact = new Contact
            {
                UserUuid = userId,
                Name = createContactDto.Name,
                PhoneNumber = createContactDto.PhoneNumber,
                Email = createContactDto.Email,
                Comment = createContactDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Contact created with ID: {contact.ContactUuid} for user: {userId}");

            return MapToResponseDto(contact);
        }

        public async Task<ContactResponseDto> GetContactByIdAsync(Guid userId, Guid contactId)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.ContactUuid == contactId && c.UserUuid == userId);

            if (contact == null)
            {
                throw new KeyNotFoundException($"Контакт с ID {contactId} не найден");
            }

            return MapToResponseDto(contact);
        }

        public async Task<List<ContactResponseDto>> GetAllContactsAsync(Guid userId)
        {
            _logger.LogInformation($"Getting all contacts for user: {userId}");

            var contacts = await _context.Contacts
                .Where(c => c.UserUuid == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            _logger.LogInformation($"Found {contacts.Count} contacts for user: {userId}");

            return contacts.Select(MapToResponseDto).ToList();
        }

        public async Task<ContactResponseDto> UpdateContactAsync(Guid userId, Guid contactId, UpdateContactDto updateContactDto)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.ContactUuid == contactId && c.UserUuid == userId);

            if (contact == null)
            {
                throw new KeyNotFoundException($"Контакт с ID {contactId} не найден");
            }

            if (updateContactDto.Name != null) contact.Name = updateContactDto.Name;
            if (updateContactDto.PhoneNumber != null) contact.PhoneNumber = updateContactDto.PhoneNumber;
            if (updateContactDto.Email != null) contact.Email = updateContactDto.Email;
            if (updateContactDto.Comment != null) contact.Comment = updateContactDto.Comment;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Contact updated: {contactId}");

            return MapToResponseDto(contact);
        }

        public async Task<ContactResponseDto?> DeleteContactAsync(Guid userId, Guid contactId)
        {
            var contact = await _context.Contacts
                .FirstOrDefaultAsync(c => c.ContactUuid == contactId && c.UserUuid == userId);
            if (contact == null) return null;

            contact.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToResponseDto(contact);
        }

        public async Task<List<ContactResponseDto>> SearchContactsAsync(Guid userId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await GetAllContactsAsync(userId);
            }

            var contacts = await _context.Contacts
                .Where(c => c.UserUuid == userId && 
                           (c.Name.Contains(searchTerm) || 
                            (c.PhoneNumber != null && c.PhoneNumber.Contains(searchTerm)) ||
                            (c.Email != null && c.Email.Contains(searchTerm)) ||
                            (c.Comment != null && c.Comment.Contains(searchTerm))))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return contacts.Select(MapToResponseDto).ToList();
        }

        private ContactResponseDto MapToResponseDto(Contact contact)
        {
            return new ContactResponseDto
            {
                ContactUuid = contact.ContactUuid,
                UserUuid = contact.UserUuid,
                Name = contact.Name,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                Comment = contact.Comment,
                CreatedAt = contact.CreatedAt.ToUnixMs(),
                DeletedAt = contact.DeletedAt.ToUnixMs()
            };
        }
    }
}