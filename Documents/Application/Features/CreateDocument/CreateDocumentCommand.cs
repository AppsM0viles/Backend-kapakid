using System;
using MediatR;
using FinTrackBack.Documents.Domain.ValueObjects;
using FinTrackBack.Documents.Application.DTOs;

namespace FinTrackBack.Documents.Application.Features.CreateDocument
{
    public class CreateDocumentCommand : IRequest<DocumentDto>
    {
        public Guid UserId { get; set; }
        public string DocumentNumber { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public DocumentType Type { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string? FilePath { get; set; }
    }
}