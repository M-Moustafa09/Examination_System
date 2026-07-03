using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");

        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasDefaultValueSql("NEWID()");

        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(q => q.Explanation)
            .HasMaxLength(2000);

        builder.Property(q => q.OrderIndex).HasDefaultValue(1);
        builder.Property(q => q.IsDeleted).HasDefaultValue(false);

        builder.HasOne(q => q.Quiz)
            .WithMany(qz => qz.Questions)
            .HasForeignKey(q => q.QuizId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}