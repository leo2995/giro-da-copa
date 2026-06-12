using GiroDaCopa.Application.Features.Admin.Dtos;
using MediatR;

namespace GiroDaCopa.Application.Features.Admin.Commands.GeneratePasswordResetToken;

public sealed record GeneratePasswordResetTokenCommand(Guid UserId, string FrontendBaseUrl)
    : IRequest<PasswordResetLinkDto?>;
