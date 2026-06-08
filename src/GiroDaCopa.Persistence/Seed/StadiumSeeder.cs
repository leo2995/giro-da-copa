using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Persistence.Seed;

public static class StadiumSeeder
{
    private sealed record StadiumSeed(
        string CountryCode,
        string CityName,
        string StateProvince,
        string StadiumName,
        int Capacity);

    public static async Task SeedAsync(GiroDaCopaDbContext context)
    {
        var existingStadiums = await context.Stadiums
            .Select(x => x.Name)
            .ToHashSetAsync();

        var pending = GetStadiums()
            .Where(x => !existingStadiums.Contains(x.StadiumName))
            .ToList();

        if (pending.Count == 0)
            return;

        var countries = await context.Countries.ToDictionaryAsync(x => x.FifaCode);
        var cities = await context.Cities
            .Include(x => x.Country)
            .ToListAsync();

        var cityByKey = cities.ToDictionary(
            x => $"{x.Country.FifaCode}:{x.Name}");

        foreach (var seed in pending)
        {
            var cityKey = $"{seed.CountryCode}:{seed.CityName}";

            if (!cityByKey.TryGetValue(cityKey, out var city))
            {
                city = new City(
                    countries[seed.CountryCode].Id,
                    seed.CityName,
                    seed.StateProvince);

                context.Cities.Add(city);
                await context.SaveChangesAsync();
                cityByKey[cityKey] = city;
            }

            context.Stadiums.Add(
                new Stadium(city.Id, seed.StadiumName, seed.Capacity));
        }

        await context.SaveChangesAsync();
    }

    private static StadiumSeed[] GetStadiums() =>
    [
        new("MEX", "Mexico City", "CDMX", "Estádio Azteca", 87000),
        new("MEX", "Guadalajara", "JAL", "Estádio Akron", 49000),
        new("MEX", "Monterrey", "NL", "Estádio BBVA", 53500),
        new("USA", "Atlanta", "GA", "Mercedes-Benz Stadium", 71000),
        new("USA", "San Francisco", "CA", "Levi's Stadium", 68500),
        new("USA", "Los Angeles", "CA", "SoFi Stadium", 70240),
        new("USA", "Seattle", "WA", "Lumen Field", 69000),
        new("USA", "New York/New Jersey", "NJ", "MetLife Stadium", 82500),
        new("USA", "Boston", "MA", "Gillette Stadium", 65878),
        new("USA", "Philadelphia", "PA", "Lincoln Financial Field", 69596),
        new("USA", "Miami", "FL", "Hard Rock Stadium", 65326),
        new("USA", "Houston", "TX", "NRG Stadium", 72220),
        new("USA", "Kansas City", "MO", "Arrowhead Stadium", 76416),
        new("USA", "Dallas", "TX", "AT&T Stadium", 80000),
        new("CAN", "Toronto", "ON", "BMO Field", 45000),
        new("CAN", "Vancouver", "BC", "BC Place", 54500),
    ];
}
