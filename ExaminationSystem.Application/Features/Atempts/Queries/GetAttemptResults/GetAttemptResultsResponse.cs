namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptResults;

public record GetAttemptResultsResponse(
    Guid AttemptId,
    Guid QuizId,
    string QuizTitle,
    decimal Score,
    bool Passed,
    string Status,
    int TotalQuestions,
    int CorrectCount,
    IReadOnlyList<AttemptQuestionResultItem> PerQuestion
);

public record AttemptQuestionResultItem(
    Guid QuestionId,
    string QuestionText,
    Guid? StudentAnswerOptionId,
    string? StudentAnswerText,
    Guid CorrectAnswerOptionId,
    string CorrectAnswerText,
    bool IsCorrect,
    string? Explanation
);
