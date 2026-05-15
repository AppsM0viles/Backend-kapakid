using MediatR;

namespace FinTrackBack.Authentication.Application.Features.Authentication.Login;

public class LoginUserQuery : IRequest<LoginUserResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}