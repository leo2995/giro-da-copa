using GiroDaCopa.Domain.Common;
using GiroDaCopa.Domain.Enums;

namespace GiroDaCopa.Domain.Entities;

public sealed class BroadcastChannel : AuditableEntity
{
    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public BroadcastType Type { get; private set; }

    public string LogoColor { get; private set; } = string.Empty;

    public string UrlPlaceholder { get; private set; } = string.Empty;

    public ICollection<MatchBroadcast> Matches { get; private set; }
        = new List<MatchBroadcast>();

    private BroadcastChannel()
    {
    }

    public BroadcastChannel(
        string code,
        string name,
        BroadcastType type,
        string logoColor,
        string urlPlaceholder)
    {
        Id = Guid.NewGuid();

        Code = code;
        Name = name;
        Type = type;
        LogoColor = logoColor;
        UrlPlaceholder = urlPlaceholder;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
