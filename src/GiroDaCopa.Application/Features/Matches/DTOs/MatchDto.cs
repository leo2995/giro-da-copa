namespace GiroDaCopa.Application.Features.Matches.DTOs;

public sealed record MatchDto(
    Guid Id,
    string HomeTeam,
    string AwayTeam,
    string Stadium,
    DateTime KickoffAt,
    string Status,
    int? HomeGoals,
    int? AwayGoals);