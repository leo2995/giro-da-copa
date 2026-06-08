using GiroDaCopa.Persistence.Context;
using Microsoft.Extensions.DependencyInjection;

namespace GiroDaCopa.Persistence.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<GiroDaCopaDbContext>();

        await CountrySeeder.SeedAsync(context);
        await BroadcastSeeder.SeedAsync(context);
        await TournamentSeeder.SeedAsync(context);
        await GroupSeeder.SeedAsync(context);
        await StadiumSeeder.SeedAsync(context);
        await GroupMatchSeeder.SeedAsync(context);
    }
}
