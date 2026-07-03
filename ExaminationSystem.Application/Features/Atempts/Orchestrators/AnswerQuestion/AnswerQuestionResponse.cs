namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.AnswerQuestion;

public record AnswerQuestionResponse(Guid QuestionId, Guid SelectedOptionId, DateTime AnsweredAt);