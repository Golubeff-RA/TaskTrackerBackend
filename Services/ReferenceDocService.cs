using Microsoft.EntityFrameworkCore;
using YourApp.Data;
using YourApp.DTOs.ReferenceDocs;
using YourApp.Extensions;
using YourApp.Models;
using YourApp.Services.Interfaces;

namespace YourApp.Services
{
    public class ReferenceDocService : IReferenceDocService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReferenceDocService> _logger;

        public ReferenceDocService(ApplicationDbContext context, ILogger<ReferenceDocService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ReferenceDocResponseDto>> GetAllAsync(Guid userId)
        {
            var docs = await _context.ReferenceDocs
                .Where(d => d.UserUuid == userId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return docs.Select(Map).ToList();
        }

        public async Task<ReferenceDocResponseDto> GetByIdAsync(Guid userId, Guid docId)
        {
            var doc = await _context.ReferenceDocs
                .FirstOrDefaultAsync(d => d.DocUuid == docId && d.UserUuid == userId);

            if (doc == null)
                throw new KeyNotFoundException($"No help found with ID {docId}");

            return Map(doc);
        }

        public async Task<ReferenceDocResponseDto> CreateAsync(Guid userId, CreateReferenceDocDto dto)
        {
            var doc = new ReferenceDoc
            {
                UserUuid = userId,
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.ReferenceDocs.AddAsync(doc);
            await _context.SaveChangesAsync();

            _logger.LogInformation("ReferenceDoc {Id} created for user {UserId}", doc.DocUuid, userId);
            return Map(doc);
        }

        public async Task<ReferenceDocResponseDto> UpdateAsync(Guid userId, Guid docId, UpdateReferenceDocDto dto)
        {
            var doc = await _context.ReferenceDocs
                .FirstOrDefaultAsync(d => d.DocUuid == docId && d.UserUuid == userId);

            if (doc == null)
                throw new KeyNotFoundException($"No help found with ID {docId}");

            if (dto.Title != null) doc.Title = dto.Title;
            if (dto.Content != null) doc.Content = dto.Content;
            doc.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Map(doc);
        }

        public async Task<ReferenceDocResponseDto?> DeleteAsync(Guid userId, Guid docId)
        {
            var doc = await _context.ReferenceDocs
                .FirstOrDefaultAsync(d => d.DocUuid == docId && d.UserUuid == userId);
            if (doc == null) return null;
            doc.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Map(doc);
        }

        private static ReferenceDocResponseDto Map(ReferenceDoc d) => new()
        {
            DocUuid = d.DocUuid,
            UserUuid = d.UserUuid,
            Title = d.Title,
            Content = d.Content,
            CreatedAt = d.CreatedAt.ToUnixMs(),
            UpdatedAt = d.UpdatedAt.ToUnixMs(),
            DeletedAt = d.DeletedAt.ToUnixMs()
        };
    }
}
