using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Common.Services;
using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Domain.Enums;

namespace GiroDaCopa.Application.Common.Mapping;

internal static class MatchMapper
{
    public static MatchDto ToDto(Match match)
    {
        var notStarted = match.Status == MatchStatus.Scheduled;
        var stats = notStarted ? EmptyStats() : BuildStats(match);
        var broadcasts = match.Broadcasts
            .Select(x => BroadcastMapper.ToDto(x.BroadcastChannel))
            .ToList();

        var dto = new MatchDto(
            match.ExternalCode,
            match.Stage.Name,
            match.Group?.Name,
            TeamMapper.ToDto(match.HomeTeam),
            TeamMapper.ToDto(match.AwayTeam),
            notStarted ? null : match.Score?.HomeGoals,
            notStarted ? null : match.Score?.AwayGoals,
            FrontendEnumMapper.ToFrontendStatus(match.Status),
            FormatDatetime(match.KickoffAt),
            new DateTimeOffset(match.KickoffAt).ToUnixTimeMilliseconds(),
            match.Stadium.Name,
            match.Stadium.City.Name,
            broadcasts,
            stats,
            notStarted
                ? []
                : match.Events
                    .OrderBy(x => x.Minute)
                    .Select(ToTimelineEvent)
                    .ToList());

        return dto with
        {
            Broadcasts = BroadcastRulesService.ApplyRules(dto)
        };
    }

    private static MatchStatsDto EmptyStats() =>
        new(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

    private static MatchStatsDto BuildStats(Match match)
    {
        decimal GetStat(Guid teamId, StatisticType type) =>
            match.Statistics
                .Where(x => x.TeamId == teamId && x.StatisticType == type)
                .Select(x => x.Value)
                .FirstOrDefault();

        var homeId = match.HomeTeamId;
        var awayId = match.AwayTeamId;

        return new MatchStatsDto(
            (int)GetStat(homeId, StatisticType.Possession),
            (int)GetStat(awayId, StatisticType.Possession),
            (int)GetStat(homeId, StatisticType.Shots),
            (int)GetStat(awayId, StatisticType.Shots),
            (int)GetStat(homeId, StatisticType.ShotsOnTarget),
            (int)GetStat(awayId, StatisticType.ShotsOnTarget),
            (int)GetStat(homeId, StatisticType.Fouls),
            (int)GetStat(awayId, StatisticType.Fouls),
            (int)GetStat(homeId, StatisticType.Corners),
            (int)GetStat(awayId, StatisticType.Corners));
    }

    private static MatchTimelineEventDto ToTimelineEvent(MatchEvent matchEvent) =>
        new(
            matchEvent.Id.ToString(),
            matchEvent.Minute,
            FrontendEnumMapper.ToFrontendEventType(matchEvent.EventType),
            matchEvent.Team?.Code,
            matchEvent.PlayerName,
            matchEvent.Description,
            matchEvent.VideoUrl);

    private static string FormatDatetime(DateTime kickoffAt) =>
        kickoffAt.ToString("MMM dd, yyyy - HH:mm");
}
