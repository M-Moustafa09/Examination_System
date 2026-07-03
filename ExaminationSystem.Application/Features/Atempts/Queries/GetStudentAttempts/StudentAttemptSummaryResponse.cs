using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetStudentAttempts;

public record StudentAttemptSummaryResponse(
    Guid AttemptId,
    string QuizTitle,
    decimal? Score,
    bool? Passed,
    AttemptStatus Status,
    DateTime? SubmittedAt
);
