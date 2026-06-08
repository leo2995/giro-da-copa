namespace GiroDaCopa.Application.Common.Dtos;

public sealed record TournamentDataDto(
    IReadOnlyDictionary<string, TeamDto> Teams,
    IReadOnlyList<BroadcastChannelDto> Broadcasts,
    IReadOnlyList<MatchDto> BracketMatches,
    IReadOnlyList<MatchDto> GroupMatches,
    IReadOnlyList<GroupStandingDto> GroupStandings);
