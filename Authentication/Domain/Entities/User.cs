namespace FinTrackBack.Authentication.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public bool Premium { get; set; } = false;
}