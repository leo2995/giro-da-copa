using GiroDaCopa.Application.Features.Tournament.Queries.GetTournamentData;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TournamentController : ControllerBase
{
    private readonly IMediator _mediator;

    public TournamentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var data = await _mediator.Send(
            new GetTournamentDataQuery(),
            cancellationToken);

        return Ok(data);
    }
}
