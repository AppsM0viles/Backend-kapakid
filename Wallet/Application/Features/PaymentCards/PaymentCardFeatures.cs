using MediatR;
using Microsoft.EntityFrameworkCore;
using FinTrackBack.Wallet.Domain.Entities;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

namespace FinTrackBack.Wallet.Application.Features.PaymentCards;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

public class PaymentCardDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string ExpiryDate { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Cci { get; set; } = string.Empty;
    public decimal DebtAmount { get; set; }
    public List<TransactionDto> Transactions { get; set; } = new();
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

public class UpdatePaymentBalanceCommand : IRequest<bool>
{
    public Guid CardId { get; set; }
    public decimal NewBalance { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal TransactionAmount { get; set; }
}

public class PayDebtCommand : IRequest<bool>
{
    public Guid CardId { get; set; }
}

public class PaymentCardHandlers :
    IRequestHandler<CreatePaymentCardCommand, Guid>,
    IRequestHandler<GetPaymentCardsQuery, List<PaymentCardDto>>,
    IRequestHandler<UpdatePaymentBalanceCommand, bool>,
    IRequestHandler<PayDebtCommand, bool>
{
    private readonly FinTrackBackDbContext _context;

    public PaymentCardHandlers(FinTrackBackDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreatePaymentCardCommand request, CancellationToken cancellationToken)
    {
        var random = new Random();
        var accountNumber = random.NextInt64(10000000000000, 99999999999999).ToString();
        var cci = $"002193{accountNumber.Substring(0, 10)}";
        var hasDebt = random.NextDouble() > 0.5;
        var debtAmount = hasDebt ? (decimal)Math.Round(random.NextDouble() * 500 + 50, 2) : 0m;

        var card = new PaymentCard
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
            FullNumber = request.FullNumber,
            Brand = request.Brand,
            Balance = request.Balance,
            ExpiryDate = request.ExpiryDate,
            Cvv = request.Cvv,
            AccountNumber = accountNumber,
            Cci = cci,
            DebtAmount = debtAmount,
            CreatedAt = DateTime.UtcNow
        };

        await _context.PaymentCards.AddAsync(card, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return card.Id;
    }

    public async Task<List<PaymentCardDto>> Handle(GetPaymentCardsQuery request, CancellationToken cancellationToken)
    {
        var cards = await _context.PaymentCards
            .Where(c => c.UserId == request.UserId)
            .ToListAsync(cancellationToken);

        var result = new List<PaymentCardDto>();

        foreach (var c in cards)
        {
            var transactions = await _context.PaymentTransactions
                .Where(t => t.PaymentCardId == c.Id)
                .OrderByDescending(t => t.Date)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    Amount = t.Amount,
                    Date = t.Date
                })
                .ToListAsync(cancellationToken);

            result.Add(new PaymentCardDto
            {
                Id = c.Id,
                UserId = c.UserId,
                FullNumber = c.FullNumber,
                Brand = c.Brand,
                Balance = c.Balance,
                ExpiryDate = c.ExpiryDate,
                Cvv = c.Cvv,
                AccountNumber = c.AccountNumber,
                Cci = c.Cci,
                DebtAmount = c.DebtAmount,
                Transactions = transactions
            });
        }

        return result;
    }

    public async Task<bool> Handle(UpdatePaymentBalanceCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.PaymentCards.FindAsync(new object[] { request.CardId }, cancellationToken);
        if (card == null) return false;
        
        card.Balance = request.NewBalance;

        var transaction = new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            PaymentCardId = card.Id,
            Description = request.Description,
            Amount = request.TransactionAmount,
            Date = DateTime.UtcNow
        };

        await _context.PaymentTransactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> Handle(PayDebtCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.PaymentCards.FindAsync(new object[] { request.CardId }, cancellationToken);
        if (card == null || card.Balance < card.DebtAmount) return false;

        var transaction = new PaymentTransaction
        {
            Id = Guid.NewGuid(),
            PaymentCardId = card.Id,
            Description = "Pago de Deuda",
            Amount = -card.DebtAmount,
            Date = DateTime.UtcNow
        };

        card.Balance -= card.DebtAmount;
        card.DebtAmount = 0m;

        await _context.PaymentTransactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}