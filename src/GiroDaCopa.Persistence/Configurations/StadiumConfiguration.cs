using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class StadiumConfiguration
    : IEntityTypeConfiguration<Stadium>
{
    public void Configure(
        EntityTypeBuilder<Stadium> builder)
    {
        builder.ToTable("stadiums");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.HasOne(x => x.City)
            .WithMany(x => x.Stadiums)
            .HasForeignKey(x => x.CityId);
    }
}