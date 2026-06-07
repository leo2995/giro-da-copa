using GiroDaCopa.Application.Features.Matches.DTOs;
using MediatR;

namespace GiroDaCopa.Application.Features.Matches.Queries.GetMatches;

public sealed record GetMatchesQuery()
    : IRequest<IEnumerable<MatchDto>>;