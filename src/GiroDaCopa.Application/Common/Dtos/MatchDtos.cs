namespace GiroDaCopa.Application.Common.Dtos;

public sealed record MatchStatsDto(
    int PossessionA,
    int PossessionB,
    int ShotsA,
    int ShotsB,
    int ShotsOnTargetA,
    int ShotsOnTargetB,
    int FoulsA,
    int FoulsB,
    int CornersA,
    int CornersB);

public sealed record MatchTimelineEventDto(
    string Id,
    int Minute,
    string Type,
    string? TeamId,
    string? Player,
    string? Detail,
    string? VideoUrl);

public sealed record MatchDto(
    string Id,
    string Stage,
    string? Group,
    TeamDto TeamA,
    TeamDto TeamB,
    int? ScoreA,
    int? ScoreB,
    string Status,
    string Datetime,
    long Timestamp,
    string Stadium,
    string City,
    IReadOnlyList<BroadcastChannelDto> Broadcasts,
    MatchStatsDto Stats,
    IReadOnlyList<MatchTimelineEventDto> Timeline);
