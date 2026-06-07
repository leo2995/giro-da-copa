using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class City : AuditableEntity
{
    public Guid CountryId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string StateProvince { get; private set; } = string.Empty;

    public Country Country { get; private set; } = null!;

    private City()
    {
    }

    public ICollection<Stadium> Stadiums { get; private set; }
        = new List<Stadium>();
    public City(
        Guid countryId,
        string name,
        string stateProvince)
    {
        Id = Guid.NewGuid();

        CountryId = countryId;

        Name = name;

        StateProvince = stateProvince;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }
}