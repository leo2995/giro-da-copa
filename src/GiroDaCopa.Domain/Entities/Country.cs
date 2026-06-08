using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Country : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;

    public string IsoCode { get; private set; } = string.Empty;

    public string FifaCode { get; private set; } = string.Empty;

    public string FlagEmoji { get; private set; } = string.Empty;

    private Country()
    {
    }

    public Country(
        string name,
        string isoCode,
        string fifaCode,
        string flagEmoji = "")
    {
        Id = Guid.NewGuid();

        Name = name;

        IsoCode = isoCode;

        FifaCode = fifaCode;

        FlagEmoji = flagEmoji;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }

    public ICollection<City> Cities { get; private set; }
    = new List<City>();
    public ICollection<Team> Teams { get; private set; }
    = new List<Team>();
}