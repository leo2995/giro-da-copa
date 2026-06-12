namespace GiroDaCopa.Application.Features.Auth.Commands.ResetPassword;

public enum ResetPasswordResultStatus
{
    Success,
    InvalidToken,
    ExpiredToken
}

public sealed record ResetPasswordResult(ResetPasswordResultStatus Status);
