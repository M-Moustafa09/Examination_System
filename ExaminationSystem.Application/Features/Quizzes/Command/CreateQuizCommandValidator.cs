using FluentValidation;

namespace ExaminationSystem.Application.Features.Quizzes.Command;

public class CreateQuizCommandValidator : AbstractValidator<CreateQuizCommand>
{
    public CreateQuizCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 5 characters long.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.");

        RuleFor(x => x.DiplomaId)
            .NotEmpty().WithMessage("DiplomaId is required.");

        RuleFor(x => x.DurationMinutes)
            .NotEmpty().WithMessage("Duration is required.");

        RuleFor(x => x.PassScore)
            .NotNull()
            .WithMessage("PassScore is required.")
            .InclusiveBetween(0, 100)
            .WithMessage("PassScore must be between 0 and 100.");

        RuleFor(x => x.MaxAttempts)
            .GreaterThan(0)
            .When(x => x.MaxAttempts.HasValue)
            .WithMessage("MaxAttempts must be greater than 0 if specified.");
    }
}