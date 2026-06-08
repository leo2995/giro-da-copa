using GiroDaCopa.Application.Common.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Matches.Queries.GetMatchByCode;

public sealed record GetMatchByCodeQuery(string Code)
    : IRequest<MatchDto?>;
