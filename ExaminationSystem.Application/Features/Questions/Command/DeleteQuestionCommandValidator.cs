using FluentValidation;

namespace ExaminationSystem.Application.Features.Questions.Command;

public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
{
    public DeleteQuestionCommandValidator()
    {
        RuleFor(x => x.QuestionId)
            .GreaterThan(0);
    }
}
