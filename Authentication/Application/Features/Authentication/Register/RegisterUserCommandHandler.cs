using FinTrackBack.Authentication.Application.Interfaces;
using FinTrackBack.Authentication.Domain.Entities;
using FinTrackBack.Authentication.Infrastructure.Persistence.DbContext;
using MediatR;

namespace FinTrackBack.Authentication.Application.Features.Authentication.Register;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly FinTrackBackDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(FinTrackBackDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var passwordHash = _passwordHasher.HashPassword(request.Password);
        
        var user = new User
        {
            Id = Guid.NewGuid(), 
            Nombre = request.Nombre,
            Email = request.Email,
            PasswordHash = passwordHash,
            Premium = false 
        };
        
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return user.Id;
    }
}
