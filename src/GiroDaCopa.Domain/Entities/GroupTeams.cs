namespace GiroDaCopa.Domain.Entities;

public sealed class GroupTeam
{
    public Guid GroupId { get; private set; }

    public Guid TeamId { get; private set; }

    public int Played { get; private set; }

    public int Won { get; private set; }

    public int Drawn { get; private set; }

    public int Lost { get; private set; }

    public int GoalsFor { get; private set; }

    public int GoalsAgainst { get; private set; }

    public int Points => Won * 3 + Drawn;

    public int GoalsDifference => GoalsFor - GoalsAgainst;

    public Group Group { get; private set; } = null!;

    public Team Team { get; private set; } = null!;

    private GroupTeam()
    {
    }

    public GroupTeam(
        Guid groupId,
        Guid teamId,
        int played = 0,
        int won = 0,
        int drawn = 0,
        int lost = 0,
        int goalsFor = 0,
        int goalsAgainst = 0)
    {
        GroupId = groupId;
        TeamId = teamId;
        Played = played;
        Won = won;
        Drawn = drawn;
        Lost = lost;
        GoalsFor = goalsFor;
        GoalsAgainst = goalsAgainst;
    }

    public void ResetStandings()
    {
        Played = 0;
        Won = 0;
        Drawn = 0;
        Lost = 0;
        GoalsFor = 0;
        GoalsAgainst = 0;
    }

    public void ApplyResult(int goalsFor, int goalsAgainst)
    {
        Played++;
        GoalsFor += goalsFor;
        GoalsAgainst += goalsAgainst;

        if (goalsFor > goalsAgainst)
            Won++;
        else if (goalsFor < goalsAgainst)
            Lost++;
        else
            Drawn++;
    }
}