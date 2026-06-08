namespace GiroDaCopa.Domain.Entities;

public sealed class MatchScore
{
    public Guid MatchId { get; private set; }

    public int HomeGoals { get; private set; }

    public int AwayGoals { get; private set; }

    public int? HomePenaltyGoals { get; private set; }

    public int? AwayPenaltyGoals { get; private set; }

    public Match Match { get; private set; } = null!;

    private MatchScore()
    {
    }

    public MatchScore(
        Guid matchId,
        int homeGoals,
        int awayGoals,
        int? homePenaltyGoals = null,
        int? awayPenaltyGoals = null)
    {
        MatchId = matchId;
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
        HomePenaltyGoals = homePenaltyGoals;
        AwayPenaltyGoals = awayPenaltyGoals;
    }

    public void Update(
        int homeGoals,
        int awayGoals,
        int? homePenaltyGoals = null,
        int? awayPenaltyGoals = null)
    {
        HomeGoals = homeGoals;
        AwayGoals = awayGoals;
        HomePenaltyGoals = homePenaltyGoals;
        AwayPenaltyGoals = awayPenaltyGoals;
    }
}