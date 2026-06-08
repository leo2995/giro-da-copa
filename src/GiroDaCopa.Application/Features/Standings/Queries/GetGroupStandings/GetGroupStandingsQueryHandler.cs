using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Mapping;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Standings.Queries.GetGroupStandings;

public sealed class GetGroupStandingsQueryHandler
    : IRequestHandler<GetGroupStandingsQuery, IReadOnlyList<GroupStandingDto>>
{
    private readonly GiroDaCopaDbContext _context;

    public GetGroupStandingsQueryHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<GroupStandingDto>> Handle(
        GetGroupStandingsQuery request,
        CancellationToken cancellationToken)
    {
        var groups = await _context.Groups
            .AsNoTracking()
            .Include(x => x.Teams)
                .ThenInclude(x => x.Team)
                    .ThenInclude(x => x.Country)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return groups
            .Select(group => new GroupStandingDto(
                group.Name,
                group.Teams
                    .OrderByDescending(x => x.Points)
                    .ThenByDescending(x => x.GoalsDifference)
                    .ThenByDescending(x => x.GoalsFor)
                    .Select(entry => new TeamStandingDto(
                        TeamMapper.ToDto(entry.Team),
                        entry.Played,
                        entry.Won,
                        entry.Drawn,
                        entry.Lost,
                        entry.GoalsFor,
                        entry.GoalsAgainst,
                        entry.GoalsDifference,
                        entry.Points))
                    .ToList()))
            .ToList();
    }
}
