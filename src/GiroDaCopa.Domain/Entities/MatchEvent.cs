using GiroDaCopa.Domain.Common;
using GiroDaCopa.Domain.Enums;

namespace GiroDaCopa.Domain.Entities;

public sealed class MatchEvent : AuditableEntity
{
    public Guid MatchId { get; private set; }

    public Guid? TeamId { get; private set; }

    public EventType EventType { get; private set; }

    public int Minute { get; private set; }

    public string? PlayerName { get; private set; }

    public string? Description { get; private set; }

    public Match Match { get; private set; } = null!;

    public Team? Team { get; private set; }

    private MatchEvent()
    {
    }

    public MatchEvent(
        Guid matchId,
        EventType eventType,
        int minute,
        Guid? teamId = null,
        string? playerName = null,
        string? description = null)
    {
        Id = Guid.NewGuid();

        MatchId = matchId;

        TeamId = teamId;

        EventType = eventType;

        Minute = minute;

        PlayerName = playerName;

        Description = description;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }
}