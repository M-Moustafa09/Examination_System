namespace ExaminationSystem.Application.Features.Quizzes.Queries.CheckQuizId;

public record CheckQuizIdResponse(bool IsFound, int MaxAttempts, int DurationMinutes);