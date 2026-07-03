using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptById;

public class GetAttemptByIdResponse
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

    public ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

    public int DurationMinutes { get; set; }
    public decimal PassScore { get; set; }
}


