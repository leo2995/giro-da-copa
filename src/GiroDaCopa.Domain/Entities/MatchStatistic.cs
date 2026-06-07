using GiroDaCopa.Domain.Common;
using GiroDaCopa.Domain.Enums;

namespace GiroDaCopa.Domain.Entities;

public sealed class MatchStatistic : AuditableEntity
{
    public Guid MatchId { get; private set; }

    public Guid TeamId { get; private set; }

    public StatisticType StatisticType { get; private set; }

    public decimal Value { get; private set; }

    public Match Match { get; private set; } = null!;

    public Team Team { get; private set; } = null!;

    private MatchStatistic()
    {
    }

    public MatchStatistic(
        Guid matchId,
        Guid teamId,
        StatisticType statisticType,
        decimal value)
    {
        Id = Guid.NewGuid();

        MatchId = matchId;

        TeamId = teamId;

        StatisticType = statisticType;

        Value = value;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }
}