using Microsoft.EntityFrameworkCore;
using GiroDaCopa.Domain.Entities;

namespace GiroDaCopa.Persistence.Context;

public sealed class GiroDaCopaDbContext : DbContext
{
    public GiroDaCopaDbContext(
        DbContextOptions<GiroDaCopaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Country> Countries => Set<Country>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Stadium> Stadiums => Set<Stadium>();

    public DbSet<Team> Teams => Set<Team>();

    public DbSet<Tournament> Tournaments => Set<Tournament>();

    public DbSet<Stage> Stages => Set<Stage>();

    public DbSet<Group> Groups => Set<Group>();

    public DbSet<GroupTeam> GroupTeams => Set<GroupTeam>();
    public DbSet<Match> Matches => Set<Match>();

    public DbSet<MatchScore> MatchScores => Set<MatchScore>();

    public DbSet<MatchEvent> MatchEvents => Set<MatchEvent>();

    public DbSet<MatchStatistic> MatchStatistics
    => Set<MatchStatistic>();

    public DbSet<BroadcastChannel> BroadcastChannels
        => Set<BroadcastChannel>();

    public DbSet<MatchBroadcast> MatchBroadcasts
        => Set<MatchBroadcast>();

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(GiroDaCopaDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}