using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Quizzes;

public record DiplomaQuizzesQueryResponse(
    Guid QuizId,
    string Title,
    int DurationMinutes,
    decimal PassScore,
    int? MaxAttempts,
    int AttemptCount,
    decimal? LastScore,
    QuizStatus Status);