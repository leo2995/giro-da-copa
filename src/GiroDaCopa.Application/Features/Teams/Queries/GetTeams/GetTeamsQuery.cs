using GiroDaCopa.Application.Common.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Teams.Queries.GetTeams;

public sealed record GetTeamsQuery
    : IRequest<IReadOnlyDictionary<string, TeamDto>>;
