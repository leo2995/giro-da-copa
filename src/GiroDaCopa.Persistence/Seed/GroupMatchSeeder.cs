using System.Globalization;
using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Persistence.Seed;

public static class GroupMatchSeeder
{
    private static readonly TimeZoneInfo BrazilTimeZone = GetBrazilTimeZone();

    private sealed record MatchSeed(
        string Code,
        string GroupName,
        string HomeTeamCode,
        string AwayTeamCode,
        string StadiumName,
        DateTime KickoffAt);

    public static async Task SeedAsync(GiroDaCopaDbContext context)
    {
        var seeds = GetMatches();
        var seedCodes = seeds
            .Select(x => x.Code)
            .ToHashSet();

        var existingMatches = await context.Matches
            .Where(x => seedCodes.Contains(x.ExternalCode))
            .ToDictionaryAsync(x => x.ExternalCode);

        foreach (var seed in seeds)
        {
            if (existingMatches.TryGetValue(seed.Code, out var existingMatch)
                && existingMatch.KickoffAt != seed.KickoffAt)
            {
                existingMatch.UpdateKickoffAt(seed.KickoffAt);
            }
        }

        var pending = seeds
            .Where(x => !existingMatches.ContainsKey(x.Code))
            .ToList();

        if (pending.Count == 0)
        {
            await context.SaveChangesAsync();
            return;
        }

        var tournament = await context.Tournaments.FirstAsync();
        var groupStage = await context.Stages.FirstAsync(x => x.Name == "Group");
        var groups = await context.Groups.ToDictionaryAsync(x => x.Name);
        var teams = await context.Teams.ToDictionaryAsync(x => x.Code);
        var stadiums = await context.Stadiums.ToDictionaryAsync(x => x.Name);

        foreach (var seed in pending)
        {
            var match = new Match(
                tournament.Id,
                groupStage.Id,
                stadiums[seed.StadiumName].Id,
                teams[seed.HomeTeamCode].Id,
                teams[seed.AwayTeamCode].Id,
                seed.KickoffAt,
                seed.Code,
                groups[seed.GroupName].Id);

            context.Matches.Add(match);
        }

        await context.SaveChangesAsync();
    }

    private static MatchSeed[] GetMatches() =>
    [
        M("gm-1", "Group A", "MEX", "RSA", "Estádio Azteca", "2026-06-11T16:00:00.000Z"),
        M("gm-2", "Group A", "KOR", "CZE", "Estádio Akron", "2026-06-11T23:00:00.000Z"),
        M("gm-3", "Group A", "CZE", "RSA", "Mercedes-Benz Stadium", "2026-06-18T13:00:00.000Z"),
        M("gm-4", "Group A", "MEX", "KOR", "Estádio Akron", "2026-06-18T22:00:00.000Z"),
        M("gm-5", "Group A", "CZE", "MEX", "Estádio Azteca", "2026-06-24T22:00:00.000Z"),
        M("gm-6", "Group A", "RSA", "KOR", "Estádio BBVA", "2026-06-24T22:00:00.000Z"),
        M("gm-7", "Group B", "CAN", "BIH", "BMO Field", "2026-06-12T16:00:00.000Z"),
        M("gm-8", "Group B", "QAT", "SUI", "Levi's Stadium", "2026-06-13T16:00:00.000Z"),
        M("gm-9", "Group B", "SUI", "BIH", "SoFi Stadium", "2026-06-18T16:00:00.000Z"),
        M("gm-10", "Group B", "CAN", "QAT", "BC Place", "2026-06-18T19:00:00.000Z"),
        M("gm-11", "Group B", "SUI", "CAN", "BC Place", "2026-06-24T16:00:00.000Z"),
        M("gm-12", "Group B", "BIH", "QAT", "Lumen Field", "2026-06-24T16:00:00.000Z"),
        M("gm-13", "Group C", "BRA", "MAR", "MetLife Stadium", "2026-06-13T19:00:00.000Z"),
        M("gm-14", "Group C", "HAI", "SCO", "Gillette Stadium", "2026-06-13T22:00:00.000Z"),
        M("gm-15", "Group C", "SCO", "MAR", "Gillette Stadium", "2026-06-19T19:00:00.000Z"),
        M("gm-16", "Group C", "BRA", "HAI", "Lincoln Financial Field", "2026-06-19T22:00:00.000Z"),
        M("gm-17", "Group C", "SCO", "BRA", "Hard Rock Stadium", "2026-06-24T19:00:00.000Z"),
        M("gm-18", "Group C", "MAR", "HAI", "Mercedes-Benz Stadium", "2026-06-24T19:00:00.000Z"),
        M("gm-19", "Group D", "USA", "PAR", "SoFi Stadium", "2026-06-12T22:00:00.000Z"),
        M("gm-20", "Group D", "AUS", "TUR", "BC Place", "2026-06-14T01:00:00.000Z"),
        M("gm-21", "Group D", "TUR", "PAR", "Levi's Stadium", "2026-06-19T01:00:00.000Z"),
        M("gm-22", "Group D", "USA", "AUS", "Lumen Field", "2026-06-19T16:00:00.000Z"),
        M("gm-23", "Group D", "TUR", "USA", "SoFi Stadium", "2026-06-25T23:00:00.000Z"),
        M("gm-24", "Group D", "PAR", "AUS", "Levi's Stadium", "2026-06-25T23:00:00.000Z"),
        M("gm-25", "Group E", "GER", "CUW", "NRG Stadium", "2026-06-14T14:00:00.000Z"),
        M("gm-26", "Group E", "CIV", "ECU", "Lincoln Financial Field", "2026-06-14T20:00:00.000Z"),
        M("gm-27", "Group E", "GER", "CIV", "BMO Field", "2026-06-20T17:00:00.000Z"),
        M("gm-28", "Group E", "ECU", "CUW", "Arrowhead Stadium", "2026-06-20T21:00:00.000Z"),
        M("gm-29", "Group E", "ECU", "GER", "MetLife Stadium", "2026-06-25T17:00:00.000Z"),
        M("gm-30", "Group E", "CUW", "CIV", "Lincoln Financial Field", "2026-06-25T17:00:00.000Z"),
        M("gm-31", "Group F", "NED", "JPN", "AT&T Stadium", "2026-06-14T17:00:00.000Z"),
        M("gm-32", "Group F", "SWE", "TUN", "Estádio BBVA", "2026-06-14T23:00:00.000Z"),
        M("gm-33", "Group F", "NED", "SWE", "NRG Stadium", "2026-06-20T14:00:00.000Z"),
        M("gm-34", "Group F", "TUN", "JPN", "Estádio BBVA", "2026-06-21T01:00:00.000Z"),
        M("gm-35", "Group F", "TUN", "NED", "Arrowhead Stadium", "2026-06-25T20:00:00.000Z"),
        M("gm-36", "Group F", "JPN", "SWE", "AT&T Stadium", "2026-06-25T20:00:00.000Z"),
        M("gm-37", "Group G", "BEL", "EGY", "Lumen Field", "2026-06-15T16:00:00.000Z"),
        M("gm-38", "Group G", "IRN", "NZL", "SoFi Stadium", "2026-06-15T22:00:00.000Z"),
        M("gm-39", "Group G", "BEL", "IRN", "SoFi Stadium", "2026-06-21T16:00:00.000Z"),
        M("gm-40", "Group G", "NZL", "EGY", "BC Place", "2026-06-21T22:00:00.000Z"),
        M("gm-41", "Group G", "EGY", "IRN", "Lumen Field", "2026-06-26T00:00:00.000Z"),
        M("gm-42", "Group G", "NZL", "BEL", "BC Place", "2026-06-26T00:00:00.000Z"),
        M("gm-43", "Group H", "ESP", "CPV", "Mercedes-Benz Stadium", "2026-06-15T13:00:00.000Z"),
        M("gm-44", "Group H", "KSA", "URU", "Hard Rock Stadium", "2026-06-15T19:00:00.000Z"),
        M("gm-45", "Group H", "ESP", "KSA", "Mercedes-Benz Stadium", "2026-06-21T13:00:00.000Z"),
        M("gm-46", "Group H", "URU", "CPV", "Hard Rock Stadium", "2026-06-21T19:00:00.000Z"),
        M("gm-47", "Group H", "URU", "ESP", "Estádio Akron", "2026-06-26T21:00:00.000Z"),
        M("gm-48", "Group H", "CPV", "KSA", "NRG Stadium", "2026-06-26T21:00:00.000Z"),
        M("gm-49", "Group I", "FRA", "SEN", "MetLife Stadium", "2026-06-16T16:00:00.000Z"),
        M("gm-50", "Group I", "IRQ", "NOR", "Gillette Stadium", "2026-06-16T19:00:00.000Z"),
        M("gm-51", "Group I", "FRA", "IRQ", "Lincoln Financial Field", "2026-06-22T18:00:00.000Z"),
        M("gm-52", "Group I", "NOR", "SEN", "MetLife Stadium", "2026-06-22T21:00:00.000Z"),
        M("gm-53", "Group I", "NOR", "FRA", "Gillette Stadium", "2026-06-26T16:00:00.000Z"),
        M("gm-54", "Group I", "SEN", "IRQ", "BMO Field", "2026-06-26T16:00:00.000Z"),
        M("gm-55", "Group J", "ARG", "ALG", "Arrowhead Stadium", "2026-06-16T22:00:00.000Z"),
        M("gm-56", "Group J", "AUT", "JOR", "Levi's Stadium", "2026-06-17T01:00:00.000Z"),
        M("gm-57", "Group J", "ARG", "AUT", "AT&T Stadium", "2026-06-22T14:00:00.000Z"),
        M("gm-58", "Group J", "JOR", "ALG", "Levi's Stadium", "2026-06-23T00:00:00.000Z"),
        M("gm-59", "Group J", "JOR", "ARG", "AT&T Stadium", "2026-06-27T23:00:00.000Z"),
        M("gm-60", "Group J", "ALG", "AUT", "Arrowhead Stadium", "2026-06-27T23:00:00.000Z"),
        M("gm-61", "Group K", "POR", "COD", "NRG Stadium", "2026-06-17T14:00:00.000Z"),
        M("gm-62", "Group K", "UZB", "COL", "Estádio Azteca", "2026-06-17T23:00:00.000Z"),
        M("gm-63", "Group K", "POR", "UZB", "NRG Stadium", "2026-06-23T14:00:00.000Z"),
        M("gm-64", "Group K", "COL", "COD", "Estádio Akron", "2026-06-23T23:00:00.000Z"),
        M("gm-65", "Group K", "COL", "POR", "Hard Rock Stadium", "2026-06-27T20:30:00.000Z"),
        M("gm-66", "Group K", "COD", "UZB", "Mercedes-Benz Stadium", "2026-06-27T20:30:00.000Z"),
        M("gm-67", "Group L", "ENG", "CRO", "AT&T Stadium", "2026-06-17T17:00:00.000Z"),
        M("gm-68", "Group L", "GHA", "PAN", "BMO Field", "2026-06-17T20:00:00.000Z"),
        M("gm-69", "Group L", "ENG", "GHA", "Gillette Stadium", "2026-06-23T17:00:00.000Z"),
        M("gm-70", "Group L", "PAN", "CRO", "BMO Field", "2026-06-23T20:00:00.000Z"),
        M("gm-71", "Group L", "PAN", "ENG", "MetLife Stadium", "2026-06-27T18:00:00.000Z"),
        M("gm-72", "Group L", "CRO", "GHA", "Lincoln Financial Field", "2026-06-27T18:00:00.000Z"),
    ];

    private static MatchSeed M(
        string code,
        string group,
        string home,
        string away,
        string stadium,
        string kickoffAt) =>
        new(code, group, home, away, stadium, ParseBrazilKickoff(kickoffAt));

    private static DateTime ParseBrazilKickoff(string kickoffAt)
    {
        var localText = kickoffAt.TrimEnd('Z');
        var local = DateTime.SpecifyKind(
            DateTime.Parse(localText, CultureInfo.InvariantCulture),
            DateTimeKind.Unspecified);

        return TimeZoneInfo.ConvertTimeToUtc(local, BrazilTimeZone);
    }

    private static TimeZoneInfo GetBrazilTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
        }
    }
}
