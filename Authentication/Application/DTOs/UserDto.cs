namespace FinTrackBack.Authentication.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Premium { get; set; }
}