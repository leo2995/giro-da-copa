namespace GiroDaCopa.Application.Features.Admin.Dtos;

public sealed record PasswordResetLinkDto(
    string Token,
    string ResetUrl,
    DateTime ExpiresAt,
    string Username);
