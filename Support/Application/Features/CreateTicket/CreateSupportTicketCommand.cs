using FinTrackBack.Support.Application.DTOs;
using FinTrackBack.Support.Domain.ValueObjects;
using MediatR;

namespace FinTrackBack.Support.Application.Features.CreateTicket;

public record CreateSupportTicketCommand(
    Guid UserId,
    string Title,
    string Description,
    TicketPriority Priority
) : IRequest<SupportTicketDto>;