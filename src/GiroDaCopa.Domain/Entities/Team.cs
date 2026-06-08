using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Team : AuditableEntity
{
    public Guid CountryId { get; private set; }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string PrimaryColor { get; private set; } = string.Empty;

    public Country Country { get; private set; } = null!;

    private Team()
    {
    }

    public Team(
        Guid countryId,
        string code,
        string name,
        string primaryColor)
    {
        Id = Guid.NewGuid();

        CountryId = countryId;

        Code = code;

        Name = name;

        PrimaryColor = primaryColor;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }
    public ICollection<GroupTeam> Groups { get; private set; }
    = new List<GroupTeam>();
}