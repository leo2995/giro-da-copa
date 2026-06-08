using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
public sealed class GroupConfiguration
    : IEntityTypeConfiguration<Group>
{
    public void Configure(
        EntityTypeBuilder<Group> builder)
    {
        builder.ToTable("groups");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasOne(x => x.Tournament)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.TournamentId);
    }
}