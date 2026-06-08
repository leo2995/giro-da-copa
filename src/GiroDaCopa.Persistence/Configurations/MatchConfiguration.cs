using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class MatchConfiguration
    : IEntityTypeConfiguration<Match>
{
    public void Configure(
        EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("matches");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.ExternalCode)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(x => x.ExternalCode)
            .IsUnique();

        builder.HasOne(x => x.Group)
            .WithMany()
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(x => x.Tournament)
            .WithMany()
            .HasForeignKey(x => x.TournamentId);

        builder.HasOne(x => x.Stage)
            .WithMany()
            .HasForeignKey(x => x.StageId);

        builder.HasOne(x => x.Stadium)
            .WithMany()
            .HasForeignKey(x => x.StadiumId);

        builder.HasOne(x => x.HomeTeam)
            .WithMany()
            .HasForeignKey(x => x.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.AwayTeam)
            .WithMany()
            .HasForeignKey(x => x.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WinnerTeam)
            .WithMany()
            .HasForeignKey(x => x.WinnerTeamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}