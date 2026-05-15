using FinTrackBack.Authentication.Application.DTOs;

namespace FinTrackBack.Authentication.Application.Features.Authentication.Login;

public class LoginUserResponse
{
    public UserDto User { get; set; } = null!;
    public string Token { get; set; } = string.Empty;
}