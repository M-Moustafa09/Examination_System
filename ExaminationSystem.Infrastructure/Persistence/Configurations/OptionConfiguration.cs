using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.ToTable("Options");

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).HasDefaultValueSql("NEWID()");

        builder.Property(o => o.Text)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(o => o.IsCorrect).HasDefaultValue(false);
        builder.Property(o => o.IsDeleted).HasDefaultValue(false);

        builder.HasOne(o => o.Question)
            .WithMany(q => q.Options)
            .HasForeignKey(o => o.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}