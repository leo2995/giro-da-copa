using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class CountryConfiguration
    : IEntityTypeConfiguration<Country>
{
    public void Configure(
        EntityTypeBuilder<Country> builder)
    {
        builder.ToTable("countries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.IsoCode)
            .HasMaxLength(3)
            .IsRequired();

        builder.Property(x => x.FifaCode)
            .HasMaxLength(5)
            .IsRequired();

        builder.Property(x => x.FlagEmoji)
            .HasMaxLength(10)
            .IsRequired();
    }
}