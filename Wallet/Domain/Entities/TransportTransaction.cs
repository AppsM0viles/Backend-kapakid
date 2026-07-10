namespace FinTrackBack.Wallet.Domain.Entities;

public class TransportTransaction
{
    public Guid Id { get; set; }
    public Guid TransportCardId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}