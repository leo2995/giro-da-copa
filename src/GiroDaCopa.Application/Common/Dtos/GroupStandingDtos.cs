namespace GiroDaCopa.Application.Common.Dtos;

public sealed record TeamStandingDto(
    TeamDto Team,
    int Played,
    int Won,
    int Drawn,
    int Lost,
    int GoalsFor,
    int GoalsAgainst,
    int GoalsDifference,
    int Points);

public sealed record GroupStandingDto(
    string GroupName,
    IReadOnlyList<TeamStandingDto> Standings);
