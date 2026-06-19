using MediatR;
using Microsoft.EntityFrameworkCore;
using FinTrackBack.Wallet.Domain.Entities;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

namespace FinTrackBack.Wallet.Application.Features.PaymentCards;

public class PaymentCardDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string ExpiryDate { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}

public class CreatePaymentCardCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string FullNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string ExpiryDate { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
}

public class GetPaymentCardsQuery : IRequest<List<PaymentCardDto>>
{
    public Guid UserId { get; set; }
}

public class PaymentCardHandlers :
    IRequestHandler<CreatePaymentCardCommand, Guid>,
    IRequestHandler<GetPaymentCardsQuery, List<PaymentCardDto>>
{
    private readonly FinTrackBackDbContext _context;

    public PaymentCardHandlers(FinTrackBackDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePaymentCardCommand request, CancellationToken cancellationToken)
    {
        var card = new PaymentCard
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            FullNumber = request.FullNumber,
            Brand = request.Brand,
            Balance = request.Balance,
            ExpiryDate = request.ExpiryDate,
            Cvv = request.Cvv,
            CreatedAt = DateTime.UtcNow
        };

        await _context.PaymentCards.AddAsync(card, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return card.Id;
    }

    public async Task<List<PaymentCardDto>> Handle(GetPaymentCardsQuery request, CancellationToken cancellationToken)
    {
        return await _context.PaymentCards
            .Where(c => c.UserId == request.UserId)
            .Select(c => new PaymentCardDto
            {
                Id = c.Id,
                UserId = c.UserId,
                FullNumber = c.FullNumber,
                Brand = c.Brand,
                Balance = c.Balance,
                ExpiryDate = c.ExpiryDate,
                Cvv = c.Cvv
            }).ToListAsync(cancellationToken);
    }
}