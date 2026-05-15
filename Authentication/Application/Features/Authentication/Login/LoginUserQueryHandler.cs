using FinTrackBack.Authentication.Application.DTOs;
using FinTrackBack.Authentication.Application.Interfaces;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FinTrackBack.Authentication.Application.Features.Authentication.Login;

public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, LoginUserResponse>
{
    private readonly FinTrackBackDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserQueryHandler(FinTrackBackDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginUserResponse> Handle(LoginUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        
        if (user == null)
        {
            throw new Exception("Email o contraseña inválidos."); 
        }
        
        var passwordIsValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);
        
        if (!passwordIsValid)
        {
            throw new Exception("Email o contraseña inválidos.");
        }
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        
        var userDto = new UserDto
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Email = user.Email,
            Premium = user.Premium
        };
        
        return new LoginUserResponse
        {
            User = userDto,
            Token = token
        };
    }
}