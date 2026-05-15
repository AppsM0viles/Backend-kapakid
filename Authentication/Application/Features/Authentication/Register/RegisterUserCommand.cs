using MediatR;

namespace FinTrackBack.Authentication.Application.Features.Authentication.Register;

public class RegisterUserCommand : IRequest<Guid> 
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}