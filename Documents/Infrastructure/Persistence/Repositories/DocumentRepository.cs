using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinTrackBack.Documents.Domain.Entities;
using FinTrackBack.Documents.Domain.Interfaces;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace FinTrackBack.Documents.Infrastructure.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly FinTrackBackDbContext _context;

        public DocumentRepository(FinTrackBackDbContext context)
        {
            _context = context;
        }

        public async Task<Document?> GetByIdAsync(Guid id)
        {
            return await _context.Documents
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Document>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Documents
                .Where(d => d.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Document document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
        }
    }
}