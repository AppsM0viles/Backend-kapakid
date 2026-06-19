using MediatR;
using Microsoft.EntityFrameworkCore;
using FinTrackBack.Wallet.Domain.Entities;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

namespace FinTrackBack.Wallet.Application.Features.TransportCards;

public class TransportCardDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime LastRechargeDate { get; set; }
}

public class CreateTransportCardCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string CardNumber { get; set; } = string.Empty;
}

public class GetTransportCardsQuery : IRequest<List<TransportCardDto>>
{
    public Guid UserId { get; set; }
}

public class TransportCardHandlers :
    IRequestHandler<CreateTransportCardCommand, Guid>,
    IRequestHandler<GetTransportCardsQuery, List<TransportCardDto>>
{
    private readonly FinTrackBackDbContext _context;

    public TransportCardHandlers(FinTrackBackDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateTransportCardCommand request, CancellationToken cancellationToken)
    {
        var card = new TransportCard
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            Type = request.Type,
            Balance = request.Balance,
            CardNumber = request.CardNumber,
            LastRechargeDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _context.TransportCards.AddAsync(card, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return card.Id;
    }

    public async Task<List<TransportCardDto>> Handle(GetTransportCardsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TransportCards
            .Where(c => c.UserId == request.UserId)
            .Select(c => new TransportCardDto
            {
                Id = c.Id,
                UserId = c.UserId,
                Type = c.Type,
                Balance = c.Balance,
                CardNumber = c.CardNumber,
                LastRechargeDate = c.LastRechargeDate
            }).ToListAsync(cancellationToken);
    }
}