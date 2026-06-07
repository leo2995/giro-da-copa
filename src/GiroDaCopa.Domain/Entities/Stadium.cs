using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Stadium : AuditableEntity
{
    public Guid CityId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public int Capacity { get; private set; }

    public City City { get; private set; } = null!;

    private Stadium()
    {
    }

    public Stadium(
        Guid cityId,
        string name,
        int capacity)
    {
        Id = Guid.NewGuid();

        CityId = cityId;

        Name = name;

        Capacity = capacity;

        CreatedAt = DateTime.UtcNow;

        UpdatedAt = DateTime.UtcNow;
    }
}