using System;
using FinTrackBack.Documents.Domain.ValueObjects;

namespace FinTrackBack.Documents.Domain.Entities
{
    public class Document
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public string DocumentNumber { get; private set; } = default!;
        public string FullName { get; private set; } = default!;

        public DocumentType Type { get; private set; }
        public DocumentStatus Status { get; private set; }

        public DateTime IssueDate { get; private set; }
        public DateTime? ExpirationDate { get; private set; }

        public string? FilePath { get; private set; }

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // Constructor vacío para EF Core
        private Document() { }

        public Document(
            Guid userId,
            string documentNumber,
            string fullName,
            DocumentType type,
            DateTime issueDate,
            DateTime? expirationDate,
            string? filePath
        )
        {
            Id = Guid.NewGuid();
            UserId = userId;
            DocumentNumber = documentNumber;
            FullName = fullName;
            Type = type;
            Status = DocumentStatus.Pending;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            FilePath = filePath;
            CreatedAt = DateTime.UtcNow;
        }

        public void Verify()
        {
            Status = DocumentStatus.Verified;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Reject()
        {
            Status = DocumentStatus.Rejected;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}