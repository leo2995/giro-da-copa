using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Persistence.Seed;

public static class GroupSeeder
{
    private sealed record StandingSeed(string TeamCode);

    private sealed record GroupSeed(string Name, StandingSeed[] Standings);

    public static async Task SeedAsync(GiroDaCopaDbContext context)
    {
        if (await context.Groups.CountAsync() >= 12)
            return;

        var tournament = await context.Tournaments.FirstOrDefaultAsync();
        if (tournament is null)
            return;

        var teamByCode = await context.Teams
            .ToDictionaryAsync(x => x.Code);

        var existingNames = await context.Groups
            .Select(x => x.Name)
            .ToListAsync();

        foreach (var groupSeed in GetGroups())
        {
            if (existingNames.Contains(groupSeed.Name))
                continue;

            var group = new Group(tournament.Id, groupSeed.Name);
            context.Groups.Add(group);
            await context.SaveChangesAsync();

            context.GroupTeams.AddRange(
                groupSeed.Standings.Select(standing =>
                    new GroupTeam(
                        group.Id,
                        teamByCode[standing.TeamCode].Id)));
        }

        await context.SaveChangesAsync();
    }

    public static async Task ResetStandingsAsync(GiroDaCopaDbContext context)
    {
        var entries = await context.GroupTeams.ToListAsync();

        foreach (var entry in entries)
            entry.ResetStandings();

        await context.SaveChangesAsync();
    }

    private static GroupSeed[] GetGroups() =>
    [
        Group("Group A", S("MEX"), S("KOR"), S("RSA"), S("CZE")),
        Group("Group B", S("CAN"), S("SUI"), S("BIH"), S("QAT")),
        Group("Group C", S("BRA"), S("MAR"), S("SCO"), S("HAI")),
        Group("Group D", S("USA"), S("TUR"), S("PAR"), S("AUS")),
        Group("Group E", S("GER"), S("ECU"), S("CIV"), S("CUW")),
        Group("Group F", S("NED"), S("JPN"), S("SWE"), S("TUN")),
        Group("Group G", S("BEL"), S("EGY"), S("IRN"), S("NZL")),
        Group("Group H", S("ESP"), S("URU"), S("KSA"), S("CPV")),
        Group("Group I", S("FRA"), S("SEN"), S("NOR"), S("IRQ")),
        Group("Group J", S("ARG"), S("AUT"), S("ALG"), S("JOR")),
        Group("Group K", S("POR"), S("COL"), S("COD"), S("UZB")),
        Group("Group L", S("ENG"), S("CRO"), S("GHA"), S("PAN"))
    ];

    private static GroupSeed Group(string name, params StandingSeed[] standings) =>
        new(name, standings);

    private static StandingSeed S(string teamCode) => new(teamCode);
}
