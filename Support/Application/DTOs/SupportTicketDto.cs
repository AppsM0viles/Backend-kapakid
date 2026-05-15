using FinTrackBack.Support.Domain.ValueObjects;

namespace FinTrackBack.Support.Application.DTOs;

public class SupportTicketDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}