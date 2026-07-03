using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;


public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
{
    public void Configure(EntityTypeBuilder<QuizAttempt> builder)
    {
        builder.ToTable("QuizAttempts");

        builder.HasKey(qa => qa.Id);
        builder.Property(qa => qa.Id).HasDefaultValueSql("NEWID()");

        builder.Property(qa => qa.StudentId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(qa => qa.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(AttemptStatus.InProgress);

        builder.Property(qa => qa.Score).HasPrecision(5, 2);
        builder.Property(qa => qa.IsDeleted).HasDefaultValue(false);

        // Partial unique index - only one in_progress attempt per student per quiz
        builder.HasIndex(qa => new { qa.StudentId, qa.QuizId })
            .IsUnique()
            .HasDatabaseName("UQ_QuizAttempts_StudentId_QuizId_InProgress");

        // Index for student attempt history
        builder.HasIndex(qa => new { qa.StudentId, qa.SubmittedAt })
            .IsDescending(false, true)
            .HasDatabaseName("IX_QuizAttempts_StudentId_SubmittedAt");

        // Index for admin analytics
        builder.HasIndex(qa => new { qa.QuizId, qa.Status })
            .HasDatabaseName("IX_QuizAttempts_QuizId_Status");

        builder.HasOne(qa => qa.Student)
            .WithMany(u => u.QuizAttempts)
            .HasForeignKey(qa => qa.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(qa => qa.Quiz)
            .WithMany(q => q.QuizAttempts)
            .HasForeignKey(qa => qa.QuizId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}