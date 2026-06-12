using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using GiroDaCopa.Application.Common.Services;
using GiroDaCopa.Domain.Entities;
using GiroDaCopa.Domain.Enums;
using GiroDaCopa.Persistence.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class PoolsController : ControllerBase
{
    private const int LockMinutesBeforeKickoff = 5;
    private readonly GiroDaCopaDbContext _context;
    private readonly PoolScoringService _poolScoringService;

    public PoolsController(
        GiroDaCopaDbContext context,
        PoolScoringService poolScoringService)
    {
        _context = context;
        _poolScoringService = poolScoringService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMyPools(
        [FromQuery] Guid? tournamentId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var query = _context.PoolMembers
            .Where(x => x.UserId == userId.Value)
            .Select(x => x.Pool);

        if (tournamentId.HasValue)
        {
            query = query.Where(x => x.TournamentId == tournamentId.Value);
        }

        var pools = await query
            .OrderBy(pool => pool.Name)
            .Select(pool => new PoolResponse(
                pool.Id,
                pool.Name,
                pool.InviteCode,
                pool.TournamentId,
                "/join/" + pool.InviteCode))
            .ToListAsync(cancellationToken);

        return Ok(pools);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePool(
        [FromBody] CreatePoolRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var name = request.Name.Trim();
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Nome do bolão é obrigatório.");

        var tournamentExists = await _context.Tournaments
            .AnyAsync(x => x.Id == request.TournamentId, cancellationToken);

        if (!tournamentExists)
            return BadRequest("Torneio inválido.");

        var inviteCode = await GenerateUniqueInviteCode(cancellationToken);

        var pool = new Pool(name, inviteCode, request.TournamentId, userId.Value);
        var ownerMember = new PoolMember(pool.Id, userId.Value, isOwner: true);

        _context.Pools.Add(pool);
        _context.PoolMembers.Add(ownerMember);
        await _context.SaveChangesAsync(cancellationToken);

        return Created(
            $"/api/pools/{pool.Id}",
            new PoolResponse(
                pool.Id,
                pool.Name,
                pool.InviteCode,
                pool.TournamentId,
                $"/join/{pool.InviteCode}"));
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinPool(
        [FromBody] JoinPoolRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        var inviteCode = request.InviteCode.Trim().ToUpperInvariant();
        var pool = await _context.Pools
            .FirstOrDefaultAsync(x => x.InviteCode == inviteCode, cancellationToken);

        if (pool is null)
            return NotFound("Bolão não encontrado para este código.");

        var alreadyMember = await _context.PoolMembers
            .AnyAsync(
                x => x.PoolId == pool.Id && x.UserId == userId.Value,
                cancellationToken);

        if (!alreadyMember)
        {
            _context.PoolMembers.Add(
                new PoolMember(pool.Id, userId.Value, isOwner: false));

            await _context.SaveChangesAsync(cancellationToken);
        }

        return Ok(
            new PoolResponse(
                pool.Id,
                pool.Name,
                pool.InviteCode,
                pool.TournamentId,
                $"/join/{pool.InviteCode}"));
    }

    [HttpGet("{poolId}/matches")]
    public async Task<IActionResult> GetPoolMatches(
        string poolId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        if (!TryParseRequiredGuid(poolId, "poolId", out var parsedPoolId, out var validationError))
            return BadRequest(validationError);

        if (!await IsPoolMember(parsedPoolId, userId.Value, cancellationToken))
            return Forbid();

        var pool = await _context.Pools
            .FirstOrDefaultAsync(x => x.Id == parsedPoolId, cancellationToken);

        if (pool is null)
            return NotFound("Bolão não encontrado.");

        var matches = await _context.Matches
            .Where(x => x.TournamentId == pool.TournamentId)
            .Select(x => new
            {
                x.Id,
                x.ExternalCode,
                x.KickoffAt,
                x.Status,
                HomeTeam = x.HomeTeam.Name,
                HomeTeamFlagCode = x.HomeTeam.Country.FlagEmoji,
                AwayTeam = x.AwayTeam.Name,
                AwayTeamFlagCode = x.AwayTeam.Country.FlagEmoji,
                ScoreA = x.Score == null ? (int?)null : x.Score.HomeGoals,
                ScoreB = x.Score == null ? (int?)null : x.Score.AwayGoals
            })
            .OrderBy(x => x.KickoffAt)
            .ToListAsync(cancellationToken);

        var predictions = await _context.PoolPredictions
            .Where(x => x.PoolId == parsedPoolId && x.UserId == userId.Value)
            .ToDictionaryAsync(x => x.MatchId, cancellationToken);

        var response = matches.Select(x =>
        {
            predictions.TryGetValue(x.Id, out var prediction);
            var lockAt = x.KickoffAt.AddMinutes(-LockMinutesBeforeKickoff);
            var canEdit = DateTime.UtcNow <= lockAt;
            return new PoolMatchResponse(
                x.Id,
                x.ExternalCode,
                x.KickoffAt,
                lockAt,
                x.Status.ToString(),
                x.HomeTeam,
                x.HomeTeamFlagCode,
                x.AwayTeam,
                x.AwayTeamFlagCode,
                x.ScoreA,
                x.ScoreB,
                prediction?.HomeGoals,
                prediction?.AwayGoals,
                canEdit);
        });

        return Ok(response);
    }

    [HttpPut("{poolId}/predictions/{matchId}")]
    public async Task<IActionResult> UpsertPrediction(
        string poolId,
        string matchId,
        [FromBody] UpsertPredictionRequest request,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        if (!TryParseRequiredGuid(poolId, "poolId", out var parsedPoolId, out var poolValidationError))
            return BadRequest(poolValidationError);

        if (!TryParseRequiredGuid(matchId, "matchId", out var parsedMatchId, out var matchValidationError))
            return BadRequest(matchValidationError);

        if (!await IsPoolMember(parsedPoolId, userId.Value, cancellationToken))
            return Forbid();

        var match = await _context.Matches
            .FirstOrDefaultAsync(x => x.Id == parsedMatchId, cancellationToken);

        if (match is null)
            return NotFound("Jogo não encontrado.");

        var pool = await _context.Pools
            .FirstOrDefaultAsync(x => x.Id == parsedPoolId, cancellationToken);

        if (pool is null)
            return NotFound("Bolão não encontrado.");

        if (pool.TournamentId != match.TournamentId)
            return BadRequest("Este jogo não pertence ao torneio do bolão.");

        var lockAt = match.KickoffAt.AddMinutes(-LockMinutesBeforeKickoff);
        if (DateTime.UtcNow > lockAt)
            return BadRequest("A edição do palpite é permitida até 5 minutos antes do jogo.");

        if (request.HomeGoals < 0 || request.AwayGoals < 0)
            return BadRequest("Os gols do palpite não podem ser negativos.");

        var prediction = await _context.PoolPredictions
            .FirstOrDefaultAsync(
                x => x.PoolId == parsedPoolId && x.UserId == userId.Value && x.MatchId == parsedMatchId,
                cancellationToken);

        if (prediction is null)
        {
            prediction = new PoolPrediction(
                parsedPoolId,
                userId.Value,
                parsedMatchId,
                request.HomeGoals,
                request.AwayGoals);
            _context.PoolPredictions.Add(prediction);
        }
        else
        {
            prediction.UpdateGoals(request.HomeGoals, request.AwayGoals);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(
            new
            {
                prediction.Id,
                prediction.PoolId,
                prediction.MatchId,
                prediction.HomeGoals,
                prediction.AwayGoals,
                prediction.UpdatedAt,
                lockAt
            });
    }

    [HttpGet("{poolId}/ranking")]
    public async Task<IActionResult> GetRanking(
        string poolId,
        CancellationToken cancellationToken)
    {
        var userId = GetUserId();
        if (userId is null)
            return Unauthorized();

        if (!TryParseRequiredGuid(poolId, "poolId", out var parsedPoolId, out var validationError))
            return BadRequest(validationError);

        if (!await IsPoolMember(parsedPoolId, userId.Value, cancellationToken))
            return Forbid();

        var members = await _context.PoolMembers
            .Where(x => x.PoolId == parsedPoolId)
            .Select(x => new
            {
                x.UserId,
                x.User.Username
            })
            .ToListAsync(cancellationToken);

        var predictions = await _context.PoolPredictions
            .Where(x => x.PoolId == parsedPoolId)
            .Select(x => new
            {
                x.UserId,
                x.MatchId,
                x.HomeGoals,
                x.AwayGoals
            })
            .ToListAsync(cancellationToken);

        var decidedMatches = await _context.Matches
            .Where(x =>
                x.Score != null &&
                x.Status != MatchStatus.Scheduled)
            .Select(x => new
            {
                x.Id,
                HomeGoals = x.Score!.HomeGoals,
                AwayGoals = x.Score!.AwayGoals
            })
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var ranking = members
            .Select(member =>
            {
                var userPredictions = predictions
                    .Where(x => x.UserId == member.UserId)
                    .ToList();

                var points = 0;
                var exactScores = 0;

                foreach (var prediction in userPredictions)
                {
                    if (!decidedMatches.TryGetValue(prediction.MatchId, out var match))
                        continue;

                    var score = _poolScoringService.CalculatePoints(
                        prediction.HomeGoals,
                        prediction.AwayGoals,
                        match.HomeGoals,
                        match.AwayGoals);

                    points += score;

                    if (prediction.HomeGoals == match.HomeGoals
                        && prediction.AwayGoals == match.AwayGoals)
                    {
                        exactScores += 1;
                    }
                }

                return new PoolRankingItem(member.UserId, member.Username, points, exactScores);
            })
            .OrderByDescending(x => x.Points)
            .ThenByDescending(x => x.ExactScores)
            .ThenBy(x => x.Username)
            .ToList();

        return Ok(ranking);
    }

    private async Task<bool> IsPoolMember(
        Guid poolId,
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await _context.PoolMembers
            .AnyAsync(
                x => x.PoolId == poolId && x.UserId == userId,
                cancellationToken);
    }

    private static bool TryParseRequiredGuid(
        string value,
        string fieldName,
        out Guid parsedValue,
        out string? errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value)
            || value.Equals("null", StringComparison.OrdinalIgnoreCase)
            || value.Equals("undefined", StringComparison.OrdinalIgnoreCase))
        {
            parsedValue = Guid.Empty;
            errorMessage = $"'{fieldName}' é obrigatório.";
            return false;
        }

        if (!Guid.TryParse(value, out parsedValue))
        {
            errorMessage = $"'{fieldName}' deve ser um GUID válido.";
            return false;
        }

        errorMessage = null;
        return true;
    }

    private async Task<string> GenerateUniqueInviteCode(CancellationToken cancellationToken)
    {
        while (true)
        {
            var code = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            var exists = await _context.Pools
                .AnyAsync(x => x.InviteCode == code, cancellationToken);

            if (!exists)
                return code;
        }
    }

    private Guid? GetUserId()
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return Guid.TryParse(userIdValue, out var userId) ? userId : null;
    }

    public sealed record CreatePoolRequest(string Name, Guid TournamentId);

    public sealed record JoinPoolRequest(string InviteCode);

    public sealed record UpsertPredictionRequest(int HomeGoals, int AwayGoals);

    public sealed record PoolResponse(
        Guid Id,
        string Name,
        string InviteCode,
        Guid TournamentId,
        string JoinPath);

    public sealed record PoolMatchResponse(
        Guid MatchId,
        string MatchCode,
        DateTime KickoffAt,
        DateTime LockAt,
        string Status,
        string HomeTeam,
        string HomeTeamFlagCode,
        string AwayTeam,
        string AwayTeamFlagCode,
        int? ActualHomeGoals,
        int? ActualAwayGoals,
        int? PredictedHomeGoals,
        int? PredictedAwayGoals,
        bool CanEdit);

    public sealed record PoolRankingItem(
        Guid UserId,
        string Username,
        int Points,
        int ExactScores);
}
