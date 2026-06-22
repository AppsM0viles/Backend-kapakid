namespace FinTrackBack.Wallet.Domain.Entities;

public class PaymentTransaction
{
    public Guid Id { get; set; }
    public Guid PaymentCardId { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
}