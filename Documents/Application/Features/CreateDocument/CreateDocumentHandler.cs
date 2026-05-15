using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FinTrackBack.Documents.Application.DTOs;
using FinTrackBack.Documents.Domain.Entities;
using FinTrackBack.Documents.Domain.Interfaces;

namespace FinTrackBack.Documents.Application.Features.CreateDocument
{
    public class CreateDocumentHandler : IRequestHandler<CreateDocumentCommand, DocumentDto>
    {
        private readonly IDocumentRepository _documentRepository;

        public CreateDocumentHandler(IDocumentRepository documentRepository)
        {
            _documentRepository = documentRepository;
        }

        public async Task<DocumentDto> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
        {
            var document = new Document(
                request.UserId,
                request.DocumentNumber,
                request.FullName,
                request.Type,
                request.IssueDate,
                request.ExpirationDate,
                request.FilePath
            );

            await _documentRepository.AddAsync(document);

            return new DocumentDto
            {
                Id = document.Id,
                UserId = document.UserId,
                DocumentNumber = document.DocumentNumber,
                FullName = document.FullName,
                Type = document.Type,
                Status = document.Status,
                IssueDate = document.IssueDate,
                ExpirationDate = document.ExpirationDate,
                FilePath = document.FilePath
            };
        }
    }
}