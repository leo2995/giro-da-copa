namespace GiroDaCopa.Application.Features.Matches.Dtos;

public sealed record UpdateMatchEventDto(
    int Minute,
    string Type,
    string? TeamId,
    string? Player,
    string? Detail,
    string? VideoUrl);
