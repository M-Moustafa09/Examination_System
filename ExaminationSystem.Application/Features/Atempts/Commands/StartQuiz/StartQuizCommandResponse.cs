namespace ExaminationSystem.Application.Features.Atempts.Commands.StartQuiz;

public record StartQuizCommandResponse(Guid AttemptId, Guid QuizId, string? Title, string? Instructions, int? DurationMinutes, DateTime? StartedAt, DateTime? Deadline);