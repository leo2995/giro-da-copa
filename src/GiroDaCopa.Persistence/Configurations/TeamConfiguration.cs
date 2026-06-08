using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class TeamConfiguration
    : IEntityTypeConfiguration<Team>
{
    public void Configure(
        EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("teams");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .HasMaxLength(5)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PrimaryColor)
            .HasMaxLength(20);

        builder.HasOne(x => x.Country)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.CountryId);
    }
}