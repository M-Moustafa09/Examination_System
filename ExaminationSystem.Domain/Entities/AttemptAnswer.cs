using ExaminationSystem.Domain.Commons;

namespace ExaminationSystem.Domain.Entities;

public class AttemptAnswer : BaseEntity
{
    public int Id { get; set; }

    /// <summary>
    /// FK → QuizAttempts.Id
    /// </summary>
    public Guid AttemptId { get; set; }

    /// <summary>
    /// FK → Questions.Id. Must belong to the attempt's quiz
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// FK → Options.Id. NULL if question was skipped
    /// </summary>
    public Guid? SelectedOptionId { get; set; }

    /// <summary>
    /// Derived at submit time: SelectedOptionId == Options.IsCorrect true row
    /// </summary>
    public bool? IsCorrect { get; set; }

    /// <summary>
    /// UTC. Updated on every re-answer (UPSERT)
    /// </summary>
    public DateTime AnsweredAt { get; set; }

    // Navigation properties
    public virtual QuizAttempt Attempt { get; set; } = null!;
    public virtual Question Question { get; set; } = null!;
    public virtual Option? SelectedOption { get; set; }
}
