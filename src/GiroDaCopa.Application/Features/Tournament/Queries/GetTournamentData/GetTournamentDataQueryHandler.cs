using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Features.Broadcasts.Queries.GetBroadcasts;
using GiroDaCopa.Application.Features.Matches.Queries.GetMatches;
using GiroDaCopa.Application.Features.Standings.Queries.GetGroupStandings;
using GiroDaCopa.Application.Features.Teams.Queries.GetTeams;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Tournament.Queries.GetTournamentData;

public sealed class GetTournamentDataQueryHandler
    : IRequestHandler<GetTournamentDataQuery, TournamentDataDto>
{
    private readonly IMediator _mediator;
    private readonly GiroDaCopaDbContext _context;

    public GetTournamentDataQueryHandler(
        IMediator mediator,
        GiroDaCopaDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task<TournamentDataDto> Handle(
        GetTournamentDataQuery request,
        CancellationToken cancellationToken)
    {
        var tournament = await _context.Tournaments
            .AsNoTracking()
            .OrderBy(x => x.Year)
            .FirstOrDefaultAsync(cancellationToken);
        if (tournament is null)
            throw new InvalidOperationException("Nenhum torneio cadastrado.");

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
            new TournamentDto(
                tournament.Id,
                tournament.Name,
                tournament.Year,
                tournament.StartDate,
                tournament.EndDate),
            teams,
            broadcasts,
            bracketMatches,
            groupMatches,
            standings);
    }
}
