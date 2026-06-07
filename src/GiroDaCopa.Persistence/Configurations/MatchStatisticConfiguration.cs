using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class MatchStatisticConfiguration
    : IEntityTypeConfiguration<MatchStatistic>
{
    public void Configure(
        EntityTypeBuilder<MatchStatistic> builder)
    {
        builder.ToTable("match_statistics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.StatisticType)
            .HasConversion<int>();

        builder.Property(x => x.Value)
            .HasPrecision(10, 2);

        builder.HasOne(x => x.Match)
            .WithMany(x => x.Statistics)
            .HasForeignKey(x => x.MatchId);

        builder.HasOne(x => x.Team)
            .WithMany()
            .HasForeignKey(x => x.TeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.MatchId);

        builder.HasIndex(x => x.TeamId);
    }
}