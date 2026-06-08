using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Features.Matches.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Matches.Commands.UpdateMatch;

public sealed record UpdateMatchCommand(
    string Code,
    string? Status,
    int? ScoreA,
    int? ScoreB,
    MatchStatsDto? Stats,
    IReadOnlyList<UpdateMatchEventDto>? Timeline)
    : IRequest<MatchDto?>;
