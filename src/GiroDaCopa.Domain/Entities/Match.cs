using GiroDaCopa.Domain.Common;
using GiroDaCopa.Domain.Enums;

namespace GiroDaCopa.Domain.Entities;

public sealed class Match : AuditableEntity
{
    public Guid TournamentId { get; private set; }

    public Guid StageId { get; private set; }

    public Guid? GroupId { get; private set; }

    public string ExternalCode { get; private set; } = string.Empty;

    public Guid StadiumId { get; private set; }

    public Guid HomeTeamId { get; private set; }

    public Guid AwayTeamId { get; private set; }

    public Guid? WinnerTeamId { get; private set; }

    public DateTime KickoffAt { get; private set; }

    public MatchStatus Status { get; private set; }

    public Tournament Tournament { get; private set; } = null!;

    public Stage Stage { get; private set; } = null!;

    public Group? Group { get; private set; }

    public Stadium Stadium { get; private set; } = null!;

    public Team HomeTeam { get; private set; } = null!;

    public Team AwayTeam { get; private set; } = null!;

    public Team? WinnerTeam { get; private set; }

    public MatchScore? Score { get; private set; }

    public ICollection<MatchEvent> Events { get; private set; }
        = new List<MatchEvent>();

    public ICollection<MatchStatistic> Statistics { get; private set; }
        = new List<MatchStatistic>();

    public ICollection<MatchBroadcast> Broadcasts { get; private set; }
        = new List<MatchBroadcast>();

    private Match()
    {
    }

    public Match(
        Guid tournamentId,
        Guid stageId,
        Guid stadiumId,
        Guid homeTeamId,
        Guid awayTeamId,
        DateTime kickoffAt,
        string externalCode,
        Guid? groupId = null,
        MatchStatus status = MatchStatus.Scheduled)
    {
        Id = Guid.NewGuid();

        TournamentId = tournamentId;
        StageId = stageId;
        GroupId = groupId;
        ExternalCode = externalCode;
        StadiumId = stadiumId;
        HomeTeamId = homeTeamId;
        AwayTeamId = awayTeamId;

        KickoffAt = kickoffAt;

        Status = status;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(MatchStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetWinner(Guid? winnerTeamId)
    {
        WinnerTeamId = winnerTeamId;
        UpdatedAt = DateTime.UtcNow;
    }
}