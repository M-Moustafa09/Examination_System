using ExaminationSystem.Domain.Commons;
using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Domain.Entities;

public class Diploma : BaseEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DiplomaStatus Status { get; set; } = DiplomaStatus.Draft;

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
}