using GiroDaCopa.Application.Common.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Standings.Queries.GetGroupStandings;

public sealed record GetGroupStandingsQuery
    : IRequest<IReadOnlyList<GroupStandingDto>>;
