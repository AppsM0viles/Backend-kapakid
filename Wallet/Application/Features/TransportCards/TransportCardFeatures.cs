using MediatR;
using Microsoft.EntityFrameworkCore;
using FinTrackBack.Wallet.Domain.Entities;
using FinTrackBack.Payments.Domain.Entities;
using FinTrackBack.Payments.Domain.Interfaces;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;

namespace FinTrackBack.Wallet.Application.Features.TransportCards;

public class TransportTransactionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}

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

public class UpdateTransportBalanceCommand : IRequest<bool>
{
    public Guid CardId { get; set; }
    public decimal NewBalance { get; set; }
}

public class GetTransportCardTransactionsQuery : IRequest<List<TransportTransactionDto>?>
{
    public Guid TransportCardId { get; set; }
}

public class RechargeTransportCardCommand : IRequest<RechargeTransportCardResult>
{
    public Guid TransportCardId { get; set; }
    public Guid? PaymentCardId { get; set; }
    public decimal Amount { get; set; }
}

public class RechargeTransportCardResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public decimal NewTransportBalance { get; set; }
    public Guid TransactionId { get; set; }
}

public class TransportCardHandlers :
    IRequestHandler<CreateTransportCardCommand, Guid>,
    IRequestHandler<GetTransportCardsQuery, List<TransportCardDto>>,
    IRequestHandler<UpdateTransportBalanceCommand, bool>,
    IRequestHandler<GetTransportCardTransactionsQuery, List<TransportTransactionDto>?>,
    IRequestHandler<RechargeTransportCardCommand, RechargeTransportCardResult>
{
    private readonly FinTrackBackDbContext _context;
    private readonly IPaymentRepository _paymentRepository;

    public TransportCardHandlers(FinTrackBackDbContext context, IPaymentRepository paymentRepository)
    {
        _context = context;
        _paymentRepository = paymentRepository;
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

    public async Task<bool> Handle(UpdateTransportBalanceCommand request, CancellationToken cancellationToken)
    {
        var card = await _context.TransportCards.FindAsync(new object[] { request.CardId }, cancellationToken);
        if (card == null) return false;

        card.Balance = request.NewBalance;
        card.LastRechargeDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<List<TransportTransactionDto>?> Handle(GetTransportCardTransactionsQuery request, CancellationToken cancellationToken)
    {
        var cardExists = await _context.TransportCards.AnyAsync(c => c.Id == request.TransportCardId, cancellationToken);
        if (!cardExists) return null;

        return await _context.TransportTransactions
            .Where(t => t.TransportCardId == request.TransportCardId)
            .OrderByDescending(t => t.Date)
            .Select(t => new TransportTransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Date = t.Date
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<RechargeTransportCardResult> Handle(RechargeTransportCardCommand request, CancellationToken cancellationToken)
    {
        if (request.Amount <= 0)
            return new RechargeTransportCardResult { Success = false, Message = "El monto debe ser mayor a cero." };

        var transportCard = await _context.TransportCards.FindAsync(new object[] { request.TransportCardId }, cancellationToken);
        if (transportCard == null)
            return new RechargeTransportCardResult { Success = false, Message = "Tarjeta de transporte no encontrada." };

        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var dbTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                if (request.PaymentCardId.HasValue)
                {
                    var paymentCard = await _context.PaymentCards.FindAsync(new object[] { request.PaymentCardId.Value }, cancellationToken);
                    if (paymentCard == null)
                    {
                        await dbTransaction.RollbackAsync(cancellationToken);
                        return new RechargeTransportCardResult { Success = false, Message = "Tarjeta de pago no encontrada." };
                    }

                    if (paymentCard.Balance < request.Amount)
                    {
                        await dbTransaction.RollbackAsync(cancellationToken);
                        return new RechargeTransportCardResult { Success = false, Message = "Saldo insuficiente en la tarjeta de pago." };
                    }

                    paymentCard.Balance -= request.Amount;

                    await _context.PaymentTransactions.AddAsync(new PaymentTransaction
                    {
                        Id = Guid.NewGuid(),
                        PaymentCardId = paymentCard.Id,
                        Description = "Recarga tarjeta de transporte",
                        Amount = -request.Amount,
                        Date = DateTime.UtcNow
                    }, cancellationToken);
                }

                transportCard.Balance += request.Amount;
                transportCard.LastRechargeDate = DateTime.UtcNow;

                var transportTransaction = new TransportTransaction
                {
                    Id = Guid.NewGuid(),
                    TransportCardId = transportCard.Id,
                    Description = "Recarga de saldo",
                    Amount = request.Amount,
                    Date = DateTime.UtcNow
                };
                await _context.TransportTransactions.AddAsync(transportTransaction, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                await _paymentRepository.AddPaymentAsync(new Payment
                {
                    Id = Guid.NewGuid(),
                    Servicio = $"Recarga {transportCard.Type}",
                    Monto = request.Amount,
                    Fecha = DateTime.UtcNow,
                    UserId = transportCard.UserId
                });

                await dbTransaction.CommitAsync(cancellationToken);

                return new RechargeTransportCardResult
                {
                    Success = true,
                    NewTransportBalance = transportCard.Balance,
                    TransactionId = transportTransaction.Id
                };
            }
            catch
            {
                await dbTransaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }
}