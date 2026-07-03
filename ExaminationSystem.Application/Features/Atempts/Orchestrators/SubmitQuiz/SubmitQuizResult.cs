namespace ExaminationSystem.Application.Features.Attempts.Orchestrators;

public record SubmitQuizResult(Guid AttemptId, decimal Score, bool Passed, string Status);