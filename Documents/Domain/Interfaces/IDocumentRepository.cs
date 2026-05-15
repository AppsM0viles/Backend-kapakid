using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FinTrackBack.Documents.Domain.Entities;

namespace FinTrackBack.Documents.Domain.Interfaces
{
    public interface IDocumentRepository
    {
        Task<Document?> GetByIdAsync(Guid id);
        Task<List<Document>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Document document);
        Task UpdateAsync(Document document);
    }
}