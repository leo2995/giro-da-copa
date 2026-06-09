namespace GiroDaCopa.Application.Common.Services;

public sealed class PoolScoringService
{
    public const int WinnerPoints = 10;
    public const int GoalDiffPoints = 5;
    public const int HomeGoalsPoints = 2;
    public const int AwayGoalsPoints = 2;
    public const int ExactScoreBonus = 10;

    public int CalculatePoints(
        int predictedHomeGoals,
        int predictedAwayGoals,
        int actualHomeGoals,
        int actualAwayGoals)
    {
        var points = 0;

        if (GetOutcome(predictedHomeGoals, predictedAwayGoals)
            == GetOutcome(actualHomeGoals, actualAwayGoals))
        {
            points += WinnerPoints;
        }

        if ((predictedHomeGoals - predictedAwayGoals)
            == (actualHomeGoals - actualAwayGoals))
        {
            points += GoalDiffPoints;
        }

        if (predictedHomeGoals == actualHomeGoals)
            points += HomeGoalsPoints;

        if (predictedAwayGoals == actualAwayGoals)
            points += AwayGoalsPoints;

        if (predictedHomeGoals == actualHomeGoals
            && predictedAwayGoals == actualAwayGoals)
        {
            points += ExactScoreBonus;
        }

        return points;
    }

    private static int GetOutcome(int homeGoals, int awayGoals)
    {
        if (homeGoals > awayGoals)
            return 1;

        if (awayGoals > homeGoals)
            return -1;

        return 0;
    }
}
