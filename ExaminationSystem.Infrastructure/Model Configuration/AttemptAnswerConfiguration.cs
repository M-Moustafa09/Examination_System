using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Model_Configuration;

public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
{
    public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
    {
        builder
            .HasOne(x => x.Attempt)
            .WithMany(x => x.AttemptAnswers)
            .HasForeignKey(x => x.AttemptId)
            .OnDelete(DeleteBehavior.NoAction); 

        builder
            .HasOne(x => x.Question)
            .WithMany(x => x.AttemptAnswers)
            .HasForeignKey(x => x.QuestionId)
            .OnDelete(DeleteBehavior.NoAction); 
    }
}