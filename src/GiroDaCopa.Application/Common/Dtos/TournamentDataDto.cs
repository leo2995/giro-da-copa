namespace GiroDaCopa.Application.Common.Dtos;

public sealed record TournamentDto(
    Guid Id,
    string Name,
    int Year,
    DateOnly StartDate,
    DateOnly EndDate);

public sealed record TournamentDataDto(
    TournamentDto Tournament,
    IReadOnlyDictionary<string, TeamDto> Teams,
    IReadOnlyList<BroadcastChannelDto> Broadcasts,
    IReadOnlyList<MatchDto> BracketMatches,
    IReadOnlyList<MatchDto> GroupMatches,
    IReadOnlyList<GroupStandingDto> GroupStandings);
