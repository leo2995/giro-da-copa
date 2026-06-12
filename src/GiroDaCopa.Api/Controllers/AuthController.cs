using GiroDaCopa.Application.Features.Auth.Commands.Login;
using GiroDaCopa.Application.Features.Auth.Commands.ResetPassword;
using GiroDaCopa.Application.Abstractions;
using GiroDaCopa.Persistence.Context;
using GiroDaCopa.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiroDaCopa.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GiroDaCopaDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public AuthController(
        IMediator mediator,
        GiroDaCopaDbContext context,
        IPasswordHasher passwordHasher)
    {
        _mediator = mediator;
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new LoginCommand(request.Username, request.Password),
            cancellationToken);

        return result is null ? Unauthorized() : Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var username = request.Username.Trim();
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Username e senha são obrigatórios.");

        var alreadyExists = await _context.Users
            .AnyAsync(x => x.Username == username, cancellationToken);

        if (alreadyExists)
            return Conflict("Nome de usuário já está em uso.");

        var user = new User(
            username,
            _passwordHasher.Hash(request.Password),
            "User");

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return Created(
            $"/api/users/{user.Id}",
            new { user.Id, user.Username, user.Role });
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(
            new ResetPasswordCommand(request.Token, request.NewPassword),
            cancellationToken);

        return result.Status switch
        {
            ResetPasswordResultStatus.Success => Ok(new { message = "Senha redefinida com sucesso." }),
            ResetPasswordResultStatus.ExpiredToken => BadRequest(new { message = "Token expirado. Solicite um novo link ao administrador." }),
            ResetPasswordResultStatus.InvalidToken => BadRequest(new { message = "Token inválido ou já utilizado." }),
            _ => StatusCode(500)
        };
    }

    public sealed record LoginRequest(string Username, string Password);
    public sealed record RegisterRequest(string Username, string Password);
    public sealed record ResetPasswordRequest(string Token, string NewPassword);
}
