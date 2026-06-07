namespace GiroDaCopa.Domain.Entities;

public sealed class GroupTeam
{
    public Guid GroupId { get; private set; }

    public Guid TeamId { get; private set; }

    public Group Group { get; private set; } = null!;

    public Team Team { get; private set; } = null!;

    private GroupTeam()
    {
    }

    public GroupTeam(
        Guid groupId,
        Guid teamId)
    {
        GroupId = groupId;
        TeamId = teamId;
    }
}