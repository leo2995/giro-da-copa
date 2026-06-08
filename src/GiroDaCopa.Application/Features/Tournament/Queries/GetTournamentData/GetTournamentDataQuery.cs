using GiroDaCopa.Application.Common.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Tournament.Queries.GetTournamentData;

public sealed record GetTournamentDataQuery
    : IRequest<TournamentDataDto>;
