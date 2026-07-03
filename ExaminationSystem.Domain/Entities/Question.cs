using ExaminationSystem.Domain.Commons;

namespace ExaminationSystem.Domain.Entities;

public class Question : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// FK → Quizzes.Id
    /// </summary>
    public Guid QuizId { get; set; }

    /// <summary>
    /// Question body text
    /// </summary>
    public string Text { get; set; } = null!;

    /// <summary>
    /// Shown to student after submission only. Never exposed during attempt
    /// </summary>
    public string? Explanation { get; set; }

    /// <summary>
    /// Admin ordering. Shuffled server-side per attempt
    /// </summary>
    public int OrderIndex { get; set; } = 1;

    // Navigation properties
    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ICollection<Option> Options { get; set; } = new List<Option>();
    public virtual ICollection<AttemptAnswer> AttemptAnswers { get; set; } = new List<AttemptAnswer>();
}