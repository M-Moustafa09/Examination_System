using ExaminationSystem.Application.Features.Questions.Query.ShuffleQuestions;

namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.Start_Quiz;

public record StartQuizResponse(Guid AttemptId, Guid QuizId, string? Title, string? Instructions, int? DurationMinutes, DateTime? StartedAt, DateTime? Deadline, List<ShuffleQuestionResponse>? Questions);