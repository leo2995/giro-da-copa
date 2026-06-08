using GiroDaCopa.Application.Common.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Matches.Queries.GetMatches;

public enum MatchCategory
{
    All,
    Group,
    Bracket
}

public sealed record GetMatchesQuery(MatchCategory Category = MatchCategory.All)
    : IRequest<IReadOnlyList<MatchDto>>;
