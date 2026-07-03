using ExaminationSystem.Domain.Commons;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class QuizAttempt : BaseEntity
{
    public Guid Id { get; set; }

    public string StudentId { get; set; } = null!;

    public Guid QuizId { get; set; }

    public AttemptStatus Status { get; set; } = AttemptStatus.InProgress;

    public DateTime StartTime { get; set; }

    public DateTime Deadline { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public decimal? Score { get; set; }

    public bool? Passed { get; set; }

    public virtual ApplicationUser Student { get; set; } = null!;
    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();
}
