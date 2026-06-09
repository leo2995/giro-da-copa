using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class PoolPrediction : AuditableEntity
{
    public Guid PoolId { get; private set; }

    public Guid UserId { get; private set; }

    public Guid MatchId { get; private set; }

    public int HomeGoals { get; private set; }

    public int AwayGoals { get; private set; }

    public Pool Pool { get; private set; } = null!;

    public User User { get; private set; } = null!;

    public Match Match { get; private set; } = null!;

    private PoolPrediction()
    {
    }

    public PoolPrediction(
        Guid poolId,
        Guid userId,
        Guid matchId,
        int homeGoals,
        int awayGoals)
    {
        Id = Guid.NewGuid();
        PoolId = poolId;
        UserId = userId;
        MatchId = matchId;
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateGoals(int homeGoals, int awayGoals)
    {
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
        UpdatedAt = DateTime.UtcNow;
    }
}
