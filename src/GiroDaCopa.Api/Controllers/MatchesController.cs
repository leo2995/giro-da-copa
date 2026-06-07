using GiroDaCopa.Application.Features.Matches.Queries.GetMatches;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MatchesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MatchesController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        CancellationToken cancellationToken)
    {
        var matches = await _mediator.Send(
            new GetMatchesQuery(),
            cancellationToken);

        return Ok(matches);
    }
}