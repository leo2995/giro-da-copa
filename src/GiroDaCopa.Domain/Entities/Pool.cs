using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Pool : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;

    public string InviteCode { get; private set; } = string.Empty;

    public Guid TournamentId { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public Tournament Tournament { get; private set; } = null!;

    public User CreatedByUser { get; private set; } = null!;

    public ICollection<PoolMember> Members { get; private set; }
        = new List<PoolMember>();

    public ICollection<PoolPrediction> Predictions { get; private set; }
        = new List<PoolPrediction>();

    private Pool()
    {
    }

    public Pool(
        string name,
        string inviteCode,
        Guid tournamentId,
        Guid createdByUserId)
    {
        Id = Guid.NewGuid();
        Name = name;
        InviteCode = inviteCode;
        TournamentId = tournamentId;
        CreatedByUserId = createdByUserId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
