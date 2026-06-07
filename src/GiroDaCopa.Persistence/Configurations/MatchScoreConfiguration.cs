using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class MatchScoreConfiguration
    : IEntityTypeConfiguration<MatchScore>
{
    public void Configure(
        EntityTypeBuilder<MatchScore> builder)
    {
        builder.ToTable("match_scores");

        builder.HasKey(x => x.MatchId);

        builder.HasOne(x => x.Match)
            .WithOne(x => x.Score)
            .HasForeignKey<MatchScore>(x => x.MatchId);
    }
}