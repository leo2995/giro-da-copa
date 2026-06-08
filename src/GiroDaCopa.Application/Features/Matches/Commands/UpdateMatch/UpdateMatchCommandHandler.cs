using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Extensions;
using GiroDaCopa.Application.Common.Mapping;
using GiroDaCopa.Application.Common.Services;
using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Domain.Enums;
using GiroDaCopa.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Application.Features.Matches.Commands.UpdateMatch;

public sealed class UpdateMatchCommandHandler
    : IRequestHandler<UpdateMatchCommand, MatchDto?>
{
    private readonly GiroDaCopaDbContext _context;
    private readonly GroupStandingsService _groupStandingsService;

    public UpdateMatchCommandHandler(
        GiroDaCopaDbContext context,
        GroupStandingsService groupStandingsService)
    {
        _context = context;
        _groupStandingsService = groupStandingsService;
    }

    public async Task<MatchDto?> Handle(
        UpdateMatchCommand request,
        CancellationToken cancellationToken)
    {
        var match = await _context.Matches
            .WithFullDetails(asNoTracking: false)
            .FirstOrDefaultAsync(
                x => x.ExternalCode == request.Code,
                cancellationToken);

        if (match is null)
            return null;

        if (request.Status is not null)
        {
            match.UpdateStatus(
                FrontendEnumMapper.FromFrontendStatus(request.Status));
        }

        var notStarted = match.Status == MatchStatus.Scheduled;

        if (!notStarted && (request.ScoreA is not null || request.ScoreB is not null))
        {
            var homeGoals = request.ScoreA ?? match.Score?.HomeGoals ?? 0;
            var awayGoals = request.ScoreB ?? match.Score?.AwayGoals ?? 0;

            if (match.Score is null)
            {
                _context.MatchScores.Add(
                    new MatchScore(match.Id, homeGoals, awayGoals));
            }
            else
            {
                match.Score.Update(homeGoals, awayGoals);
            }

            match.SetWinner(ResolveWinner(match, homeGoals, awayGoals));
        }

        if (!notStarted && request.Stats is not null)
        {
            UpsertStatistic(match, match.HomeTeamId, StatisticType.Possession, request.Stats.PossessionA);
            UpsertStatistic(match, match.AwayTeamId, StatisticType.Possession, request.Stats.PossessionB);
            UpsertStatistic(match, match.HomeTeamId, StatisticType.Shots, request.Stats.ShotsA);
            UpsertStatistic(match, match.AwayTeamId, StatisticType.Shots, request.Stats.ShotsB);
            UpsertStatistic(match, match.HomeTeamId, StatisticType.ShotsOnTarget, request.Stats.ShotsOnTargetA);
            UpsertStatistic(match, match.AwayTeamId, StatisticType.ShotsOnTarget, request.Stats.ShotsOnTargetB);
            UpsertStatistic(match, match.HomeTeamId, StatisticType.Fouls, request.Stats.FoulsA);
            UpsertStatistic(match, match.AwayTeamId, StatisticType.Fouls, request.Stats.FoulsB);
            UpsertStatistic(match, match.HomeTeamId, StatisticType.Corners, request.Stats.CornersA);
            UpsertStatistic(match, match.AwayTeamId, StatisticType.Corners, request.Stats.CornersB);
        }

        if (!notStarted && request.Timeline is not null)
        {
            _context.MatchEvents.RemoveRange(match.Events);
            match.Events.Clear();

            var teamCodes = request.Timeline
                .Where(x => !string.IsNullOrWhiteSpace(x.TeamId))
                .Select(x => x.TeamId!)
                .Distinct()
                .ToList();

            var teamsByCode = await _context.Teams
                .Where(x => teamCodes.Contains(x.Code))
                .ToDictionaryAsync(x => x.Code, cancellationToken);

            foreach (var item in request.Timeline)
            {
                Guid? teamId = null;

                if (!string.IsNullOrWhiteSpace(item.TeamId))
                {
                    if (!teamsByCode.TryGetValue(item.TeamId, out var team))
                        throw new InvalidOperationException(
                            $"Team '{item.TeamId}' was not found.");

                    teamId = team.Id;
                }

                var matchEvent = new MatchEvent(
                    match.Id,
                    FrontendEnumMapper.FromFrontendEventType(item.Type),
                    item.Minute,
                    teamId,
                    item.Player,
                    item.Detail,
                    item.VideoUrl);

                _context.MatchEvents.Add(matchEvent);
                match.Events.Add(matchEvent);
            }
        }

        if (match.Status == MatchStatus.Scheduled)
            ClearNotStartedMatchData(match);

        await _context.SaveChangesAsync(cancellationToken);

        if (match.GroupId is not null)
            await _groupStandingsService.RecalculateAsync(cancellationToken);

        var updated = await _context.Matches
            .WithFullDetails()
            .FirstAsync(x => x.Id == match.Id, cancellationToken);

        return MatchMapper.ToDto(updated);
    }

    private static Guid? ResolveWinner(Match match, int homeGoals, int awayGoals)
    {
        if (homeGoals > awayGoals)
            return match.HomeTeamId;

        if (awayGoals > homeGoals)
            return match.AwayTeamId;

        return null;
    }

    private void UpsertStatistic(
        Match match,
        Guid teamId,
        StatisticType type,
        int value)
    {
        var statistic = match.Statistics
            .FirstOrDefault(x => x.TeamId == teamId && x.StatisticType == type);

        if (statistic is null)
        {
            _context.MatchStatistics.Add(
                new MatchStatistic(match.Id, teamId, type, value));
            return;
        }

        statistic.UpdateValue(value);
    }

    private void ClearNotStartedMatchData(Match match)
    {
        if (match.Statistics.Count > 0)
        {
            _context.MatchStatistics.RemoveRange(match.Statistics);
            match.Statistics.Clear();
        }

        if (match.Events.Count > 0)
        {
            _context.MatchEvents.RemoveRange(match.Events);
            match.Events.Clear();
        }

        if (match.Score is not null)
            _context.MatchScores.Remove(match.Score);

        match.SetWinner(null);
    }
}
