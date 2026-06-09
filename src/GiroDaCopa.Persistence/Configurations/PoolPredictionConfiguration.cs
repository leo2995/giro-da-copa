using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class PoolPredictionConfiguration : IEntityTypeConfiguration<PoolPrediction>
{
    public void Configure(EntityTypeBuilder<PoolPrediction> builder)
    {
        builder.ToTable("pool_predictions");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.PoolId, x.UserId, x.MatchId })
            .IsUnique();

        builder.HasOne(x => x.Pool)
            .WithMany(x => x.Predictions)
            .HasForeignKey(x => x.PoolId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Match)
            .WithMany()
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
