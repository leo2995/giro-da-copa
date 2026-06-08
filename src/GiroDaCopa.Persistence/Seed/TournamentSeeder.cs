using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Domain.Enums;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Persistence.Seed;

public static class TournamentSeeder
{
    public static async Task SeedAsync(GiroDaCopaDbContext context)
    {
        if (await context.Tournaments.AnyAsync())
            return;

        var countries = await context.Countries.ToListAsync();
        var countryByCode = countries.ToDictionary(x => x.FifaCode);

        var teams = CreateTeams(countryByCode);
        context.Teams.AddRange(teams);
        await context.SaveChangesAsync();

        var teamByCode = teams.ToDictionary(x => x.Code);

        var tournament = new Tournament(
            "FIFA World Cup",
            2026,
            new DateOnly(2026, 6, 11),
            new DateOnly(2026, 7, 19));

        var groupStage = new Stage(tournament.Id, "Group", 1);
        var roundOf32 = new Stage(tournament.Id, "Round of 32", 2);

        context.Tournaments.Add(tournament);
        context.Stages.AddRange(groupStage, roundOf32);
        await context.SaveChangesAsync();

        await GroupSeeder.SeedAsync(context);
    }

    private static List<Team> CreateTeams(
        IReadOnlyDictionary<string, Country> countryByCode)
    {
        return
        [
            new(countryByCode["ARG"].Id, "ARG", "Argentina", "#74ACDF"),
            new(countryByCode["FRA"].Id, "FRA", "France", "#002395"),
            new(countryByCode["BRA"].Id, "BRA", "Brazil", "#009739"),
            new(countryByCode["GER"].Id, "GER", "Germany", "#000000"),
            new(countryByCode["JPN"].Id, "JPN", "Japan", "#BC002D"),
            new(countryByCode["ESP"].Id, "ESP", "Spain", "#C60B1E"),
            new(countryByCode["ENG"].Id, "ENG", "England", "#E00000"),
            new(countryByCode["CRO"].Id, "CRO", "Croatia", "#FF0000"),
            new(countryByCode["MAR"].Id, "MAR", "Morocco", "#C1272D"),
            new(countryByCode["POR"].Id, "POR", "Portugal", "#046A38"),
            new(countryByCode["USA"].Id, "USA", "USA", "#0A254C"),
            new(countryByCode["MEX"].Id, "MEX", "Mexico", "#006341"),
            new(countryByCode["CAN"].Id, "CAN", "Canada", "#DA291C"),
            new(countryByCode["NED"].Id, "NED", "Netherlands", "#E45E05"),
            new(countryByCode["SEN"].Id, "SEN", "Senegal", "#00853F"),
            new(countryByCode["COL"].Id, "COL", "Colombia", "#FCD116"),
            new(countryByCode["RSA"].Id, "RSA", "South Africa", "#007C5F"),
            new(countryByCode["KOR"].Id, "KOR", "South Korea", "#C11B17"),
            new(countryByCode["CZE"].Id, "CZE", "Czech Republic", "#113C7A"),
            new(countryByCode["BIH"].Id, "BIH", "Bosnia & Herzegovina", "#002395"),
            new(countryByCode["QAT"].Id, "QAT", "Qatar", "#8A1538"),
            new(countryByCode["SUI"].Id, "SUI", "Switzerland", "#D52B1E"),
            new(countryByCode["HAI"].Id, "HAI", "Haiti", "#00209F"),
            new(countryByCode["SCO"].Id, "SCO", "Scotland", "#005EB8"),
            new(countryByCode["PAR"].Id, "PAR", "Paraguay", "#D52B1E"),
            new(countryByCode["AUS"].Id, "AUS", "Australia", "#000033"),
            new(countryByCode["TUR"].Id, "TUR", "Turkey", "#E30A17"),
            new(countryByCode["CUW"].Id, "CUW", "Curaçao", "#002B7F"),
            new(countryByCode["CIV"].Id, "CIV", "Ivory Coast", "#F77F00"),
            new(countryByCode["ECU"].Id, "ECU", "Ecuador", "#FFDD00"),
            new(countryByCode["SWE"].Id, "SWE", "Sweden", "#006AA7"),
            new(countryByCode["TUN"].Id, "TUN", "Tunisia", "#E10613"),
            new(countryByCode["BEL"].Id, "BEL", "Belgium", "#ED2939"),
            new(countryByCode["EGY"].Id, "EGY", "Egypt", "#C09307"),
            new(countryByCode["IRN"].Id, "IRN", "Iran", "#239E46"),
            new(countryByCode["NZL"].Id, "NZL", "New Zealand", "#00247D"),
            new(countryByCode["CPV"].Id, "CPV", "Cape Verde", "#003893"),
            new(countryByCode["KSA"].Id, "KSA", "Saudi Arabia", "#006C35"),
            new(countryByCode["URU"].Id, "URU", "Uruguay", "#43A1D5"),
            new(countryByCode["IRQ"].Id, "IRQ", "Iraq", "#7f8c8d"),
            new(countryByCode["NOR"].Id, "NOR", "Norway", "#EF2B2D"),
            new(countryByCode["ALG"].Id, "ALG", "Algeria", "#006233"),
            new(countryByCode["AUT"].Id, "AUT", "Austria", "#ED2939"),
            new(countryByCode["JOR"].Id, "JOR", "Jordan", "#C1272D"),
            new(countryByCode["COD"].Id, "COD", "DR Congo", "#007FFF"),
            new(countryByCode["UZB"].Id, "UZB", "Uzbekistan", "#00BFFF"),
            new(countryByCode["GHA"].Id, "GHA", "Ghana", "#FCD116"),
            new(countryByCode["PAN"].Id, "PAN", "Panama", "#DA121A")
        ];
    }
}
