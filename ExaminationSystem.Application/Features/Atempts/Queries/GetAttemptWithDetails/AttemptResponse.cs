using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetails;

public record AttemptResponse(
    Guid Id,
    string StudentId,
    string Student_name,
    Guid QuizId,
    string Quiz_name,
    DateTime StartTime,
    DateTime? SubmittedAt,
    AttemptStatus Status,
    decimal? Score,
    bool? Passed,
    ICollection<AttemptAnswer> AttemptAnswers,
    int DurationMinutes,
    decimal PassScore
);