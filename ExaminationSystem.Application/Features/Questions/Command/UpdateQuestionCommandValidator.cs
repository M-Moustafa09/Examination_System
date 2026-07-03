using FluentValidation;

namespace ExaminationSystem.Application.Features.Questions.Command;

public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
{
    public UpdateQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .GreaterThan(0);

        RuleFor(x => x.Text)
            .NotEmpty()
            .MaximumLength(1000);

        RuleFor(x => x.OrderIndex)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Options)
            .NotNull()
            .Must(options => options.Count >= 2)
            .WithMessage("At least 2 options are required.")
            .Must(options => options.Count(o => o.IsCorrect) == 1)
            .WithMessage("Exactly one correct option is required.");

        RuleForEach(x => x.Options).ChildRules(option =>
        {
            option.RuleFor(x => x.Text)
                .NotEmpty()
                .MaximumLength(500);

            option.RuleFor(x => x.OrderIndex)
                .GreaterThanOrEqualTo(0);
        });
    }
}
