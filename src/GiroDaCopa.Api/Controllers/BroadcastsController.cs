using GiroDaCopa.Application.Features.Broadcasts.Queries.GetBroadcasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BroadcastsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BroadcastsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var broadcasts = await _mediator.Send(
            new GetBroadcastsQuery(),
            cancellationToken);

        return Ok(broadcasts);
    }
}
