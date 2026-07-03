using ExaminationSystem.Domain.Commons;

namespace ExaminationSystem.Domain.Entities;

public class Enrollment : BaseEntity
{
    public int Id { get; set; }

    /// <summary>
    /// FK → AspNetUsers.Id. Role must be Student
    /// </summary>
    public string StudentId { get; set; } = null!;

    /// <summary>
    /// FK → Diplomas.Id
    /// </summary>
    public Guid DiplomaId { get; set; }

    /// <summary>
    /// UTC — when the enrollment was created
    /// </summary>
    public DateTime EnrolledAt { get; set; }

    // Navigation properties
    public virtual ApplicationUser Student { get; set; } = null!;
    public virtual Diploma Diploma { get; set; } = null!;

}