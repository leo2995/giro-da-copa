using GiroDaCopa.Domain.Enums;
using GiroDaCopa.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Common.Services;

public sealed class GroupStandingsService
{
    private readonly GiroDaCopaDbContext _context;

    public GroupStandingsService(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task RecalculateAsync(CancellationToken cancellationToken = default)
    {
        var entries = await _context.GroupTeams.ToListAsync(cancellationToken);

        foreach (var entry in entries)
            entry.ResetStandings();

        var finishedGroupMatches = await _context.Matches
            .AsNoTracking()
            .Include(x => x.Score)
            .Where(x =>
                x.GroupId != null &&
                x.Status == MatchStatus.Finished &&
                x.Score != null)
            .ToListAsync(cancellationToken);

        if (finishedGroupMatches.Count == 0)
        {
            await _context.SaveChangesAsync(cancellationToken);
            return;
        }

        var lookup = entries.ToDictionary(x => (x.GroupId, x.TeamId));

        foreach (var match in finishedGroupMatches)
        {
            var groupId = match.GroupId!.Value;
            var homeGoals = match.Score!.HomeGoals;
            var awayGoals = match.Score!.AwayGoals;

            if (lookup.TryGetValue((groupId, match.HomeTeamId), out var homeEntry))
                homeEntry.ApplyResult(homeGoals, awayGoals);

            if (lookup.TryGetValue((groupId, match.AwayTeamId), out var awayEntry))
                awayEntry.ApplyResult(awayGoals, homeGoals);
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
