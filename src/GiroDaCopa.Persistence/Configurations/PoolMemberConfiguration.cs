using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class PoolMemberConfiguration : IEntityTypeConfiguration<PoolMember>
{
    public void Configure(EntityTypeBuilder<PoolMember> builder)
    {
        builder.ToTable("pool_members");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.PoolId, x.UserId })
            .IsUnique();

        builder.HasOne(x => x.Pool)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.PoolId);

        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
