using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Stage : AuditableEntity
{
    public Guid TournamentId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int OrderIndex { get; private set; }

    public Tournament Tournament { get; private set; } = null!;

    private Stage()
    {
    }

    public Stage(
        Guid tournamentId,
        string name,
        int orderIndex)
    {
        Id = Guid.NewGuid();

        TournamentId = tournamentId;
        Name = name;
        OrderIndex = orderIndex;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}