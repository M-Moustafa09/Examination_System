using ExaminationSystem.Domain.Commons;

namespace ExaminationSystem.Domain.Entities;

public class PasswordResetToken : BaseEntity
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string TokenHash { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
}