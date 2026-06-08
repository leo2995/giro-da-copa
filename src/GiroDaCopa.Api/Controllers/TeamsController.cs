using GiroDaCopa.Application.Features.Teams.Queries.GetTeams;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class TeamsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TeamsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var teams = await _mediator.Send(new GetTeamsQuery(), cancellationToken);
        return Ok(teams);
    }
}
