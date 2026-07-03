using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;


public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
{
    public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
    {
        builder.ToTable("AttemptAnswers");

        builder.HasKey(aa => aa.Id);
        builder.Property(aa => aa.Id).UseIdentityColumn();

        builder.Property(aa => aa.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(aa => new { aa.AttemptId, aa.QuestionId })
            .IsUnique()
            .HasDatabaseName("UQ_AttemptAnswers_AttemptId_QuestionId");

        builder.HasOne(aa => aa.Attempt)
            .WithMany(qa => qa.AttemptAnswers)
            .HasForeignKey(aa => aa.AttemptId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(aa => aa.Question)
            .WithMany(q => q.AttemptAnswers)
            .HasForeignKey(aa => aa.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(aa => aa.SelectedOption)
            .WithMany(o => o.AttemptAnswers)
            .HasForeignKey(aa => aa.SelectedOptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}