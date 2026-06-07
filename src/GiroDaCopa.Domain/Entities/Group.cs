using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Group : AuditableEntity
{
    public Guid TournamentId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public Tournament Tournament { get; private set; } = null!;

    public ICollection<GroupTeam> Teams { get; private set; }
        = new List<GroupTeam>();

    private Group()
    {
    }

    public Group(
        Guid tournamentId,
        string name)
    {
        Id = Guid.NewGuid();

        TournamentId = tournamentId;
        Name = name;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}