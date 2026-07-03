using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
{
    public void Configure(EntityTypeBuilder<Quiz> builder)
    {
        builder.ToTable("Quizzes");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasDefaultValueSql("NEWID()");

        builder.Property(q => q.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(q => q.Instructions)
            .HasMaxLength(2000);

        builder.Property(q => q.PassScore)
            .HasPrecision(5, 2)
            .HasDefaultValue(60.00m);

        builder.Property(q => q.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(QuizStatus.Draft);

        builder.Property(q => q.IsDeleted).HasDefaultValue(false);

        // Index for listing published quizzes per diploma
        builder.HasIndex(q => new { q.DiplomaId, q.Status })
            .HasDatabaseName("IX_Quizzes_DiplomaId_Status");

        builder.HasOne(q => q.Diploma)
            .WithMany(d => d.Quizzes)
            .HasForeignKey(q => q.DiplomaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
