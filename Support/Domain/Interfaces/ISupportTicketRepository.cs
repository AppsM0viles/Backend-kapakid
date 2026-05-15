using FinTrackBack.Support.Domain.Entities;

namespace FinTrackBack.Support.Domain.Interfaces;

public interface ISupportTicketRepository
{
    Task<SupportTicket> AddAsync(SupportTicket ticket, CancellationToken cancellationToken = default);
}