using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class BroadcastChannelConfiguration
    : IEntityTypeConfiguration<BroadcastChannel>
{
    public void Configure(EntityTypeBuilder<BroadcastChannel> builder)
    {
        builder.ToTable("broadcast_channels");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>();

        builder.Property(x => x.LogoColor)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.UrlPlaceholder)
            .HasMaxLength(500)
            .IsRequired();
    }
}
