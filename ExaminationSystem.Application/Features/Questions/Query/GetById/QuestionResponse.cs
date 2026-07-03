namespace ExaminationSystem.Application.Features.Questions.Query.GetById;

public record QuestionResponse
    (
    Guid Id,
    Guid QuizId,
    string Text,
    string? Explanation,
    int OrderIndex
);
