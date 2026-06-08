using GiroDaCopa.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Common.Extensions;

internal static class MatchQueryExtensions
{
    public static IQueryable<Match> WithFullDetails(
        this IQueryable<Match> query,
        bool asNoTracking = true)
    {
        var result = query
            .Include(x => x.Stage)
            .Include(x => x.Group)
            .Include(x => x.HomeTeam)
                .ThenInclude(x => x.Country)
            .Include(x => x.AwayTeam)
                .ThenInclude(x => x.Country)
            .Include(x => x.Stadium)
                .ThenInclude(x => x.City)
            .Include(x => x.Score)
            .Include(x => x.Events)
                .ThenInclude(x => x.Team)
            .Include(x => x.Statistics)
            .Include(x => x.Broadcasts)
                .ThenInclude(x => x.BroadcastChannel);

        return asNoTracking ? result.AsNoTracking() : result;
    }
}
