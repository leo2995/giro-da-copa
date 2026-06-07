using GiroDaCopa.Application.Features.Matches.DTOs;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace GiroDaCopa.Application.Features.Matches.Queries.GetMatches;

public sealed class GetMatchesQueryHandler
    : IRequestHandler<GetMatchesQuery, IEnumerable<MatchDto>>
{
    private readonly GiroDaCopaDbContext _context;

    public GetMatchesQueryHandler(
        GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MatchDto>> Handle(
        GetMatchesQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.Matches
            .AsNoTracking()
            .Include(x => x.HomeTeam)
            .Include(x => x.AwayTeam)
            .Include(x => x.Stadium)
            .Include(x => x.Score)
            .OrderBy(x => x.KickoffAt)
            .Select(x => new MatchDto(
                x.Id,
                x.HomeTeam.Name,
                x.AwayTeam.Name,
                x.Stadium.Name,
                x.KickoffAt,
                x.Status.ToString(),
                x.Score != null ? x.Score.HomeGoals : null,
                x.Score != null ? x.Score.AwayGoals : null))
            .ToListAsync(cancellationToken);
    }
}