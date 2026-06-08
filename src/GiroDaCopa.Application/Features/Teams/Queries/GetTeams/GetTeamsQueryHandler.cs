using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Mapping;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Teams.Queries.GetTeams;

public sealed class GetTeamsQueryHandler
    : IRequestHandler<GetTeamsQuery, IReadOnlyDictionary<string, TeamDto>>
{
    private readonly GiroDaCopaDbContext _context;

    public GetTeamsQueryHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyDictionary<string, TeamDto>> Handle(
        GetTeamsQuery request,
        CancellationToken cancellationToken)
    {
        var teams = await _context.Teams
            .AsNoTracking()
            .Include(x => x.Country)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return teams.ToDictionary(
            x => x.Code,
            TeamMapper.ToDto);
    }
}
