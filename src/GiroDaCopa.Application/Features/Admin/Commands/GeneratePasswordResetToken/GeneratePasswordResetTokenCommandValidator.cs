using FluentValidation;

namespace GiroDaCopa.Application.Features.Admin.Commands.GeneratePasswordResetToken;

public sealed class GeneratePasswordResetTokenCommandValidator
    : AbstractValidator<GeneratePasswordResetTokenCommand>
{
    public GeneratePasswordResetTokenCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("ID do usuário é obrigatório.");

        RuleFor(x => x.FrontendBaseUrl)
            .NotEmpty()
            .WithMessage("URL base do frontend é obrigatória.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("URL base do frontend deve ser uma URL válida.");
    }
}
