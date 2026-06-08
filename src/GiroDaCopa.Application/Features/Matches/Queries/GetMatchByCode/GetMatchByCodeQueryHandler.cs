using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Extensions;
using GiroDaCopa.Application.Common.Mapping;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Matches.Queries.GetMatchByCode;

public sealed class GetMatchByCodeQueryHandler
    : IRequestHandler<GetMatchByCodeQuery, MatchDto?>
{
    private readonly GiroDaCopaDbContext _context;

    public GetMatchByCodeQueryHandler(GiroDaCopaDbContext context)
    {
        _context = context;
    }

    public async Task<MatchDto?> Handle(
        GetMatchByCodeQuery request,
        CancellationToken cancellationToken)
    {
        var match = await _context.Matches
            .WithFullDetails()
            .FirstOrDefaultAsync(
                x => x.ExternalCode == request.Code,
                cancellationToken);

        return match is null ? null : MatchMapper.ToDto(match);
    }
}
