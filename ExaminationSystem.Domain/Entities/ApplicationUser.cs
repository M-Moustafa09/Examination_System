using ExaminationSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ExaminationSystem.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;

    public UserStatus AccountStatus { get; set; } = UserStatus.Pending;
    public UserRole Role { get; set; } = UserRole.Student;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? LastLoginAt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public virtual ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();
    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public virtual ICollection<QuizAttempt> QuizAttempts { get; set; } = new List<QuizAttempt>();
}
