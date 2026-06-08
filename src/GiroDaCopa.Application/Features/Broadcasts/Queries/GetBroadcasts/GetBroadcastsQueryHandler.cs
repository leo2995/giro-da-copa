using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Mapping;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Broadcasts.Queries.GetBroadcasts;

public sealed class GetBroadcastsQueryHandler
    : IRequestHandler<GetBroadcastsQuery, IReadOnlyList<BroadcastChannelDto>>
{
    private readonly GiroDaCopaDbContext _context;

    public GetBroadcastsQueryHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<BroadcastChannelDto>> Handle(
        GetBroadcastsQuery request,
        CancellationToken cancellationToken)
    {
        var channels = await _context.BroadcastChannels
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return channels
            .Select(BroadcastMapper.ToDto)
            .ToList();
    }
}
