using GiroDaCopa.Application.Features.Admin.Commands.GeneratePasswordResetToken;
using GiroDaCopa.Application.Features.Admin.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetUsersQuery(search), cancellationToken);
        return Ok(result);
    }

    [HttpPost("users/{userId:guid}/password-reset-link")]
    public async Task<IActionResult> GeneratePasswordResetLink(
        Guid userId,
        [FromBody] GeneratePasswordResetLinkRequest request,
        CancellationToken cancellationToken)
    {
        var frontendBaseUrl = request.FrontendBaseUrl
            ?? $"{Request.Scheme}://{Request.Host}";

        var result = await _mediator.Send(
            new GeneratePasswordResetTokenCommand(userId, frontendBaseUrl),
            cancellationToken);

        if (result is null)
            return NotFound("Usuário não encontrado.");

        return Ok(result);
    }

    public sealed record GeneratePasswordResetLinkRequest(string? FrontendBaseUrl);
}
