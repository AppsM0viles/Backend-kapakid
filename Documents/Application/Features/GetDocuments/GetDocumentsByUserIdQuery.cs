using MediatR;
using FinTrackBack.Documents.Application.DTOs;
using FinTrackBack.Documents.Domain.Interfaces;

namespace FinTrackBack.Documents.Application.Features.GetDocuments;

public class GetDocumentsByUserIdQuery : IRequest<List<DocumentDto>>
{
    public Guid UserId { get; set; }
}

public class GetDocumentsByUserIdQueryHandler : IRequestHandler<GetDocumentsByUserIdQuery, List<DocumentDto>>
{
    private readonly IDocumentRepository _repository;

    public GetDocumentsByUserIdQueryHandler(IDocumentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DocumentDto>> Handle(GetDocumentsByUserIdQuery request, CancellationToken cancellationToken)
    {
        var documents = await _repository.GetByUserIdAsync(request.UserId);
        return documents.Select(d => new DocumentDto
        {
            Id = d.Id,
            UserId = d.UserId,
            DocumentNumber = d.DocumentNumber,
            FullName = d.FullName,
            Type = d.Type,
            Status = d.Status,
            IssueDate = d.IssueDate,
            ExpirationDate = d.ExpirationDate,
            FilePath = d.FilePath
        }).ToList();
    }
}