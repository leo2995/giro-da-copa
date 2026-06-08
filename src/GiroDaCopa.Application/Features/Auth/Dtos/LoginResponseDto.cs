namespace GiroDaCopa.Application.Features.Auth.Dtos;

public sealed record LoginResponseDto(
    string Token,
    DateTime ExpiresAt,
    string Username,
    string Role);
