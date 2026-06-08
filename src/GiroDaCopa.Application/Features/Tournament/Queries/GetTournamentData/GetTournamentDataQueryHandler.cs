using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Features.Broadcasts.Queries.GetBroadcasts;
using GiroDaCopa.Application.Features.Matches.Queries.GetMatches;
using GiroDaCopa.Application.Features.Standings.Queries.GetGroupStandings;
using GiroDaCopa.Application.Features.Teams.Queries.GetTeams;
using MediatR;

namespace GiroDaCopa.Application.Features.Tournament.Queries.GetTournamentData;

public sealed class GetTournamentDataQueryHandler
    : IRequestHandler<GetTournamentDataQuery, TournamentDataDto>
{
    private readonly IMediator _mediator;

    public GetTournamentDataQueryHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TournamentDataDto> Handle(
        GetTournamentDataQuery request,
        CancellationToken cancellationToken)
    {
        var teams = await _mediator.Send(new GetTeamsQuery(), cancellationToken);
        var broadcasts = await _mediator.Send(new GetBroadcastsQuery(), cancellationToken);
        var groupMatches = await _mediator.Send(
            new GetMatchesQuery(MatchCategory.Group),
            cancellationToken);
        var bracketMatches = await _mediator.Send(
            new GetMatchesQuery(MatchCategory.Bracket),
            cancellationToken);
        var standings = await _mediator.Send(
            new GetGroupStandingsQuery(),
            cancellationToken);

        return new TournamentDataDto(
            teams,
            broadcasts,
            bracketMatches,
            groupMatches,
            standings);
    }
}
