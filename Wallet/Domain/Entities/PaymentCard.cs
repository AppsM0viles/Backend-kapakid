namespace FinTrackBack.Wallet.Domain.Entities;

public class PaymentCard
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string FullNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string ExpiryDate { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}