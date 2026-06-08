using GiroDaCopa.Application.Features.Standings.Queries.GetGroupStandings;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class StandingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StandingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("groups")]
    public async Task<IActionResult> GetGroups(
        CancellationToken cancellationToken)
    {
        var standings = await _mediator.Send(
            new GetGroupStandingsQuery(),
            cancellationToken);

        return Ok(standings);
    }
}
