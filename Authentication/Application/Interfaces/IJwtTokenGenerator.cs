using FinTrackBack.Authentication.Domain.Entities;

namespace FinTrackBack.Authentication.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}