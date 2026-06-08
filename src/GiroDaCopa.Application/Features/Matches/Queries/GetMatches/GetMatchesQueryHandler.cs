using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Extensions;
using GiroDaCopa.Application.Common.Mapping;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Matches.Queries.GetMatches;

public sealed class GetMatchesQueryHandler
    : IRequestHandler<GetMatchesQuery, IReadOnlyList<MatchDto>>
{
    private readonly GiroDaCopaDbContext _context;

    public GetMatchesQueryHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<MatchDto>> Handle(
        GetMatchesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.Matches.WithFullDetails();

        query = request.Category switch
        {
            MatchCategory.Group => query.Where(x => x.GroupId != null),
            MatchCategory.Bracket => query.Where(x => x.GroupId == null),
            _ => query
        };

        var matches = await query
            .OrderBy(x => x.KickoffAt)
            .ToListAsync(cancellationToken);

        return matches
            .Select(MatchMapper.ToDto)
            .ToList();
    }
}
