using FluentValidation;

namespace GiroDaCopa.Application.Features.Matches.Commands.UpdateMatch;

public sealed class UpdateMatchCommandValidator
    : AbstractValidator<UpdateMatchCommand>
{
    public UpdateMatchCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x)
            .Must(x => x.Status is not null ||
                       x.ScoreA is not null ||
                       x.ScoreB is not null ||
                       x.Stats is not null ||
                       x.Timeline is not null)
            .WithMessage("At least one field must be provided for update.");

        When(x => x.ScoreA is not null, () =>
        {
            RuleFor(x => x.ScoreA)
                .GreaterThanOrEqualTo(0);
        });

        When(x => x.ScoreB is not null, () =>
        {
            RuleFor(x => x.ScoreB)
                .GreaterThanOrEqualTo(0);
        });
    }
}
