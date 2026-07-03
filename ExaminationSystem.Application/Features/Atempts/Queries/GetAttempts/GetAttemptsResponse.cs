using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttempts;

public class GetAttemptsResponse
{
    public required string StudentId { get; set; }

    public required string StudentName { get; set; }

    public Guid QuizId { get; set; }

    public required string QuizTitle { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime? SubmittedAt { get; set; }

    public AttemptStatus Status { get; set; }

    public decimal? Score { get; set; }
    public bool? Passed { get; set; }
}

