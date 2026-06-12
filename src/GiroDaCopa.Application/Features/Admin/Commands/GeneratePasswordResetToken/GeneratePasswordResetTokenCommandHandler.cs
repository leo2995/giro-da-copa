using GiroDaCopa.Application.Features.Admin.Dtos;
using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace GiroDaCopa.Application.Features.Admin.Commands.GeneratePasswordResetToken;

public sealed class GeneratePasswordResetTokenCommandHandler
    : IRequestHandler<GeneratePasswordResetTokenCommand, PasswordResetLinkDto?>
{
    private const int TokenExpirationHours = 24;

    private readonly GiroDaCopaDbContext _context;

    public GeneratePasswordResetTokenCommandHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<PasswordResetLinkDto?> Handle(
        GeneratePasswordResetTokenCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

        if (user is null)
            return null;

        var existingTokens = await _context.PasswordResetTokens
            .Where(x => x.UserId == request.UserId && !x.IsUsed && x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var old in existingTokens)
            old.MarkAsUsed();

        var rawToken = GenerateSecureToken();
        var expiresAt = DateTime.UtcNow.AddHours(TokenExpirationHours);

        var resetToken = new PasswordResetToken(user.Id, rawToken, expiresAt);
        _context.PasswordResetTokens.Add(resetToken);

        await _context.SaveChangesAsync(cancellationToken);

        var resetUrl = $"{request.FrontendBaseUrl.TrimEnd('/')}/reset-password?token={rawToken}";

        return new PasswordResetLinkDto(rawToken, resetUrl, expiresAt, user.Username);
    }

    private static string GenerateSecureToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }
}
