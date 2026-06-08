using GiroDaCopa.Application.Features.Auth.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Username, string Password)
    : IRequest<LoginResponseDto?>;
