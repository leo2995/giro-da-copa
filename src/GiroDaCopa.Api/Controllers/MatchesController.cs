using GiroDaCopa.Application.Common.Dtos;
using GiroDaCopa.Application.Features.Matches.Commands.UpdateMatch;
using GiroDaCopa.Application.Features.Matches.Dtos;
using GiroDaCopa.Application.Features.Matches.Queries.GetMatchByCode;
using GiroDaCopa.Application.Features.Matches.Queries.GetMatches;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? category,
        CancellationToken cancellationToken)
    {
        var matchCategory = category?.ToLowerInvariant() switch
        {
            "group" => MatchCategory.Group,
            "bracket" => MatchCategory.Bracket,
            _ => MatchCategory.All
        };

        var matches = await _mediator.Send(
            new GetMatchesQuery(matchCategory),
            cancellationToken);

        return Ok(matches);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(
        string code,
        CancellationToken cancellationToken)
    {
        var match = await _mediator.Send(
            new GetMatchByCodeQuery(code),
            cancellationToken);

        return match is null ? NotFound() : Ok(match);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{code}")]
    public async Task<IActionResult> Update(
        string code,
        [FromBody] UpdateMatchRequest request,
        CancellationToken cancellationToken)
    {
        var match = await _mediator.Send(
            new UpdateMatchCommand(
                code,
                request.Status,
                request.ScoreA,
                request.ScoreB,
                request.Stats,
                request.Timeline),
            cancellationToken);

        return match is null ? NotFound() : Ok(match);
    }

    public sealed record UpdateMatchRequest(
        string? Status,
        int? ScoreA,
        int? ScoreB,
        MatchStatsDto? Stats,
        IReadOnlyList<UpdateMatchEventDto>? Timeline);
}
