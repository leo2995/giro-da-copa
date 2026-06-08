using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Domain.Enums;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Persistence.Seed;

public static class BroadcastSeeder
{
    public static async Task SeedAsync(GiroDaCopaDbContext context)
    {
        if (await context.BroadcastChannels.AnyAsync())
            return;

        var channels = new[]
        {
            new BroadcastChannel("tv_globo", "TV Globo", BroadcastType.Free, "#DC2626", "https://redeglobo.globo.com/"),
            new BroadcastChannel("caze_tv", "CazéTV", BroadcastType.Streaming, "#FF6F00", "https://www.youtube.com/@CazeTV"),
            new BroadcastChannel("grupo_globo", "Grupo Globo", BroadcastType.Free, "#0054F0", "https://globoplay.globo.com"),
            new BroadcastChannel("sbt", "SBT", BroadcastType.Free, "#5B21B6", "https://www.sbt.com.br"),
            new BroadcastChannel("nsports", "N Sports", BroadcastType.Streaming, "#10B981", "https://www.nsports.com.br")
        };

        context.BroadcastChannels.AddRange(channels);
        await context.SaveChangesAsync();
    }
}
