using FluentValidation;

namespace GiroDaCopa.Application.Features.Auth.Commands.ResetPassword;

public sealed class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage("Token é obrigatório.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("Nova senha é obrigatória.")
            .MinimumLength(6)
            .WithMessage("Nova senha deve ter pelo menos 6 caracteres.")
            .MaximumLength(100)
            .WithMessage("Nova senha deve ter no máximo 100 caracteres.");
    }
}
