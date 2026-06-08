using GiroDaCopa.Domain.Enums;

namespace GiroDaCopa.Application.Common.Mapping;

internal static class FrontendEnumMapper
{
    public static string ToFrontendStatus(MatchStatus status) =>
        status switch
        {
            MatchStatus.Scheduled => "scheduled",
            MatchStatus.Live => "live",
            MatchStatus.Finished => "finished",
            MatchStatus.Cancelled => "cancelled",
            _ => status.ToString().ToLowerInvariant()
        };

    public static string ToFrontendEventType(EventType eventType) =>
        eventType switch
        {
            EventType.Kickoff => "kickoff",
            EventType.Goal => "goal",
            EventType.YellowCard => "yellow_card",
            EventType.RedCard => "red_card",
            EventType.Substitution => "substitution",
            EventType.Var => "var",
            EventType.HalfTime => "half_time",
            EventType.FullTime => "full_time",
            _ => eventType.ToString().ToLowerInvariant()
        };

    public static string ToFrontendBroadcastType(BroadcastType type) =>
        type switch
        {
            BroadcastType.Free => "free",
            BroadcastType.Cable => "cable",
            BroadcastType.Streaming => "streaming",
            _ => type.ToString().ToLowerInvariant()
        };

    public static MatchStatus FromFrontendStatus(string status) =>
        status.ToLowerInvariant() switch
        {
            "scheduled" => MatchStatus.Scheduled,
            "live" => MatchStatus.Live,
            "finished" => MatchStatus.Finished,
            "cancelled" => MatchStatus.Cancelled,
            _ => throw new ArgumentException($"Invalid match status: {status}")
        };

    public static EventType FromFrontendEventType(string eventType) =>
        eventType.ToLowerInvariant() switch
        {
            "kickoff" => EventType.Kickoff,
            "goal" => EventType.Goal,
            "yellow_card" => EventType.YellowCard,
            "red_card" => EventType.RedCard,
            "substitution" => EventType.Substitution,
            "var" => EventType.Var,
            "half_time" => EventType.HalfTime,
            "full_time" => EventType.FullTime,
            _ => throw new ArgumentException($"Invalid event type: {eventType}")
        };
}
