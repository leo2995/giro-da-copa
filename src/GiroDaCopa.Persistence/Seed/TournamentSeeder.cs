using GiroDaCopa.Persistence.Context;

public static class SeedData
{
    public static async Task InitializeAsync(
        IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context =
            scope.ServiceProvider
                 .GetRequiredService<GiroDaCopaDbContext>();

        await TournamentSeeder.SeedAsync(context);
    }
}