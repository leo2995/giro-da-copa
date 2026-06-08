using GiroDaCopa.Application.Common.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Broadcasts.Queries.GetBroadcasts;

public sealed record GetBroadcastsQuery
    : IRequest<IReadOnlyList<BroadcastChannelDto>>;
