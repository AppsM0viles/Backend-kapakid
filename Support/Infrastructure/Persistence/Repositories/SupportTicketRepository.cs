using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;
using FinTrackBack.Support.Domain.Entities;
using FinTrackBack.Support.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinTrackBack.Support.Infrastructure.Persistence.Repositories;

public class SupportTicketRepository : ISupportTicketRepository
{
    private readonly FinTrackBackDbContext _dbContext;

    public SupportTicketRepository(FinTrackBackDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SupportTicket> AddAsync(SupportTicket ticket, CancellationToken cancellationToken = default)
    {
        _dbContext.SupportTickets.Add(ticket);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return ticket;
    }
}