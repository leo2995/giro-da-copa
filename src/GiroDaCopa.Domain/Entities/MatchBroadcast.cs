namespace GiroDaCopa.Domain.Entities;

public sealed class MatchBroadcast
{
    public Guid MatchId { get; private set; }

    public Guid BroadcastChannelId { get; private set; }

    public Match Match { get; private set; } = null!;

    public BroadcastChannel BroadcastChannel { get; private set; } = null!;

    private MatchBroadcast()
    {
    }

    public MatchBroadcast(
        Guid matchId,
        Guid broadcastChannelId)
    {
        MatchId = matchId;
        BroadcastChannelId = broadcastChannelId;
    }
}
