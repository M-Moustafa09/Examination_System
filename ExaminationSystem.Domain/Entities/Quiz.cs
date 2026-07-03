using ExaminationSystem.Domain.Commons;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class Quiz : BaseEntity
{
    public Guid Id { get; set; }

    /// <summary>
    /// FK → Diplomas.Id
    /// </summary>
    public Guid DiplomaId { get; set; }

    /// <summary>
    /// 3–200 characters
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Displayed to student before starting
    /// </summary>
    public string? Instructions { get; set; }

    /// <summary>
    /// Positive integer. Server enforces deadline
    /// </summary>
    public int DurationMinutes { get; set; }

    /// <summary>
    /// 0–100. Compared against calculated Score
    /// </summary>
    public decimal PassScore { get; set; } = 60.00m;

    /// <summary>
    /// NULL = unlimited. Enforced on StartQuiz
    /// </summary>
    public int? MaxAttempts { get; set; }

    /// <summary>
    /// draft | published. Transition via PATCH publish/unpublish
    /// </summary>
    public QuizStatus Status { get; set; } = QuizStatus.Draft;

    /// <summary>
    /// Set when Status transitions to published
    /// </summary>
    public DateTime? PublishedAt { get; set; }

    // Navigation properties
    public virtual Diploma Diploma { get; set; } = null!;
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}