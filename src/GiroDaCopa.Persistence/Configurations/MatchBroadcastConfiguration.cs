using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class MatchBroadcastConfiguration
    : IEntityTypeConfiguration<MatchBroadcast>
{
    public void Configure(EntityTypeBuilder<MatchBroadcast> builder)
    {
        builder.ToTable("match_broadcasts");

        builder.HasKey(x => new { x.MatchId, x.BroadcastChannelId });

        builder.HasOne(x => x.Match)
            .WithMany(x => x.Broadcasts)
            .HasForeignKey(x => x.MatchId);

        builder.HasOne(x => x.BroadcastChannel)
            .WithMany(x => x.Matches)
            .HasForeignKey(x => x.BroadcastChannelId);
    }
}
