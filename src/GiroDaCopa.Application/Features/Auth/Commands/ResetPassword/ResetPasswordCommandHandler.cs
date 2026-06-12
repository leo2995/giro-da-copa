using GiroDaCopa.Application.Abstractions;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler
    : IRequestHandler<ResetPasswordCommand, ResetPasswordResult>
{
    private readonly GiroDaCopaDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public ResetPasswordCommandHandler(
        GiroDaCopaDbContext context,
        IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResetPasswordResult> Handle(
        ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var resetToken = await _context.PasswordResetTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == request.Token, cancellationToken);

        if (resetToken is null || resetToken.IsUsed)
            return new ResetPasswordResult(ResetPasswordResultStatus.InvalidToken);

        if (resetToken.ExpiresAt <= DateTime.UtcNow)
            return new ResetPasswordResult(ResetPasswordResultStatus.ExpiredToken);

        var newHash = _passwordHasher.Hash(request.NewPassword);
        resetToken.User.ChangePassword(newHash);
        resetToken.MarkAsUsed();

        await _context.SaveChangesAsync(cancellationToken);

        return new ResetPasswordResult(ResetPasswordResultStatus.Success);
    }
}
