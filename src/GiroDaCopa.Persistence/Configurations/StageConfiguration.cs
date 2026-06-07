using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public sealed class StageConfiguration
    : IEntityTypeConfiguration<Stage>
{
    public void Configure(
        EntityTypeBuilder<Stage> builder)
    {
        builder.ToTable("stages");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(x => x.Tournament)
            .WithMany(x => x.Stages)
            .HasForeignKey(x => x.TournamentId);
    }
}