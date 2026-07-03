namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.AnswerQuestion;

public record AnswerQuestionRequest(
    Guid QuestionId,
    Guid SelectedOptionId
);