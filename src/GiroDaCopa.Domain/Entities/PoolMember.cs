using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class PoolMember : AuditableEntity
{
    public Guid PoolId { get; private set; }

    public Guid UserId { get; private set; }

    public bool IsOwner { get; private set; }

    public Pool Pool { get; private set; } = null!;

    public User User { get; private set; } = null!;

    private PoolMember()
    {
    }

    public PoolMember(Guid poolId, Guid userId, bool isOwner)
    {
        Id = Guid.NewGuid();
        PoolId = poolId;
        UserId = userId;
        IsOwner = isOwner;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
