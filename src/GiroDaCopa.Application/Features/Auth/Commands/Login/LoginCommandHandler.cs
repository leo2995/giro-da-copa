using GiroDaCopa.Application.Abstractions;
using GiroDaCopa.Application.Features.Auth.Dtos;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginResponseDto?>
{
    private readonly GiroDaCopaDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginCommandHandler(
        GiroDaCopaDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto?> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(
                x => x.Username == request.Username,
                cancellationToken);

        if (user is null ||
            !_passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return null;
        }

        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(
            user.Id,
            user.Username,
            user.Role);

        return new LoginResponseDto(
            token,
            expiresAt,
            user.Username,
            user.Role);
    }
}
