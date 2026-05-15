using System;
using FinTrackBack.Documents.Domain.ValueObjects;

namespace FinTrackBack.Documents.Application.DTOs
{
    public class DocumentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string DocumentNumber { get; set; } = default!;
        public string FullName { get; set; } = default!;

        public DocumentType Type { get; set; }
        public DocumentStatus Status { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string? FilePath { get; set; }
    }
}