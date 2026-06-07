
using GiroDaCopa.Domain.Common;

namespace GiroDaCopa.Domain.Entities;

public sealed class Tournament : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;

    public int Year { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public ICollection<Stage> Stages { get; private set; }
        = new List<Stage>();

    public ICollection<Group> Groups { get; private set; }
        = new List<Group>();

    private Tournament()
    {
    }

    public Tournament(
        string name,
        int year,
        DateOnly startDate,
        DateOnly endDate)
    {
        Id = Guid.NewGuid();

        Name = name;
        Year = year;
        StartDate = startDate;
        EndDate = endDate;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}