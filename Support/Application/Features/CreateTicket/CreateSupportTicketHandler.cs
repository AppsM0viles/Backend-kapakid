using FinTrackBack.Support.Application.DTOs;
using FinTrackBack.Support.Domain.Entities;
using FinTrackBack.Support.Domain.Interfaces;
using FinTrackBack.Support.Domain.ValueObjects;
using MediatR;

namespace FinTrackBack.Support.Application.Features.CreateTicket;

public class CreateSupportTicketHandler : IRequestHandler<CreateSupportTicketCommand, SupportTicketDto>
{
    private readonly ISupportTicketRepository _supportTicketRepository;

    public CreateSupportTicketHandler(ISupportTicketRepository supportTicketRepository)
    {
        _supportTicketRepository = supportTicketRepository;
    }

    public async Task<SupportTicketDto> Handle(CreateSupportTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = new SupportTicket
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _supportTicketRepository.AddAsync(ticket, cancellationToken);

        return new SupportTicketDto
        {
            Id = created.Id,
            UserId = created.UserId,
            Title = created.Title,
            Description = created.Description,
            Priority = created.Priority,
            Status = created.Status,
            CreatedAt = created.CreatedAt
        };
    }
}