using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using GiroDaCopa.Domain.Entities;

namespace GiroDaCopa.Persistence.Seed;
    
public static class CountrySeed
{
    public static async Task SeedAsync(
        GiroDaCopaDbContext context)
    {
        if (await context.Countries.AnyAsync())
            return;

        var countries = new[]
        {
            new Country("Brazil", "BR"),
            new Country("Argentina", "AR"),
            new Country("United States", "US"),
            new Country("Mexico", "MX"),
            new Country("Canada", "CA")
        };

        context.Countries.AddRange(countries);

        await context.SaveChangesAsync();
    }
}