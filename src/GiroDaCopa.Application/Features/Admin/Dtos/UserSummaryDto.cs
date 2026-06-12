namespace GiroDaCopa.Application.Features.Admin.Dtos;

public sealed record UserSummaryDto(
    Guid Id,
    string Username,
    string Role,
    DateTime CreatedAt,
    bool HasPendingPasswordReset);
