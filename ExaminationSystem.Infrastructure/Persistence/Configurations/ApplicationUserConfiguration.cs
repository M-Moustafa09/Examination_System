using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Table name is already AspNetUsers from Identity

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(UserRole.Student);

        builder.Property(u => u.AccountStatus)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(UserStatus.Pending);

        builder.Property(u => u.IsDeleted)
            .HasDefaultValue(false);

        builder.HasMany(u => u.PasswordResetTokens)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId);

        builder.HasMany(u => u.Enrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId);

        builder.HasMany(u => u.QuizAttempts)
            .WithOne(qa => qa.Student)
            .HasForeignKey(qa => qa.StudentId);
    }
}
