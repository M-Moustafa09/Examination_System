using FluentValidation;

namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.AnswerQuestion;

public sealed class AnswerQuestionOrchestratorValidator : AbstractValidator<AnswerQuestionOrchestrator>
{
    public AnswerQuestionOrchestratorValidator()
    {
        RuleFor(x => x.AttemptId)
            .NotEmpty()
            .WithMessage("Attempt ID is required.");

        RuleFor(x => x.QuestionId)
            .NotEmpty()
            .WithMessage("Question ID is required.");

        RuleFor(x => x.SelectedOptionId)
            .NotEmpty()
            .WithMessage("Selected option ID is required.");
    }
}