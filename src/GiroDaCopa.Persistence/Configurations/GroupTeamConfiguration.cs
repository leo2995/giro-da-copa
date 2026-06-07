using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GiroDaCopa.Persistence.Configurations;

public sealed class GroupTeamConfiguration
    : IEntityTypeConfiguration<GroupTeam>
{
    public void Configure(
        EntityTypeBuilder<GroupTeam> builder)
    {
        builder.ToTable("group_teams");

        builder.HasKey(x =>
            new
            {
                x.GroupId,
                x.TeamId
            });

        builder.HasOne(x => x.Group)
            .WithMany(x => x.Teams)
            .HasForeignKey(x => x.GroupId);

        builder.HasOne(x => x.Team)
            .WithMany(x => x.Groups)
            .HasForeignKey(x => x.TeamId);
    }
}