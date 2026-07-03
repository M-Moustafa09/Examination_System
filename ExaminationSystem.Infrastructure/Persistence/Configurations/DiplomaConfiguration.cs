using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class DiplomaConfiguration : IEntityTypeConfiguration<Diploma>
{
    public void Configure(EntityTypeBuilder<Diploma> builder)
    {
        builder.ToTable("Diplomas");

        builder.HasKey(d => d.Id);
        builder.Property(d => d.Id).HasDefaultValueSql("NEWID()");

        builder.Property(d => d.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(d => d.Description)
            .HasMaxLength(1000);

        builder.Property(d => d.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue(DiplomaStatus.Draft);

        builder.Property(d => d.IsDeleted).HasDefaultValue(false);
    }
}
