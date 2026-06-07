using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class MatchEventConfiguration
    : IEntityTypeConfiguration<MatchEvent>
{
    public void Configure(
        EntityTypeBuilder<MatchEvent> builder)
    {
        builder.ToTable("match_events");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EventType)
            .HasConversion<int>();

        builder.Property(x => x.Minute)
            .IsRequired();

        builder.Property(x => x.PlayerName)
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.HasOne(x => x.Match)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.MatchId);

        builder.HasOne(x => x.Team)
            .WithMany()
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}