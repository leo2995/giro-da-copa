using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class PoolConfiguration : IEntityTypeConfiguration<Pool>
{
    public void Configure(EntityTypeBuilder<Pool> builder)
    {
        builder.ToTable("pools");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(x => x.InviteCode)
            .HasMaxLength(12)
            .IsRequired();

        builder.HasIndex(x => x.InviteCode)
            .IsUnique();

        builder.HasOne(x => x.Tournament)
            .WithMany()
            .HasForeignKey(x => x.TournamentId);

        builder.HasOne(x => x.CreatedByUser)
            .WithMany()
            .HasForeignKey(x => x.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
