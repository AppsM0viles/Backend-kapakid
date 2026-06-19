namespace FinTrackBack.Wallet.Domain.Entities;

public class TransportCard
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string CardNumber { get; set; } = string.Empty;
    public DateTime LastRechargeDate { get; set; }
    public DateTime CreatedAt { get; set; }
}