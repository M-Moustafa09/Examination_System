using ExaminationSystem.Domain.Abstractions;
using MediatR;


namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.AnswerQuestion;

public record AnswerQuestionOrchestrator(
    Guid AttemptId,
    Guid QuestionId,
    Guid SelectedOptionId
) : IRequest<Result<AnswerQuestionResponse>>;

