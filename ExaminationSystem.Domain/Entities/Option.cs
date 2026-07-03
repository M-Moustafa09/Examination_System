using ExaminationSystem.Domain.Commons;

namespace ExaminationSystem.Domain.Entities;

public class Option : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// FK → Questions.Id
    /// </summary>
    public Guid QuestionId { get; set; }

    /// <summary>
    /// Option text shown to student
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Exactly 1 per question must be true. NEVER returned to student before submission
    /// </summary>
    public bool IsCorrect { get; set; }

    // Navigation properties
    public virtual Question Question { get; set; } = null!;
    public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();

}