using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).UseIdentityColumn();

        builder.Property(e => e.StudentId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(e => e.IsDeleted).HasDefaultValue(false);

        builder.HasIndex(e => new { e.StudentId, e.DiplomaId })
            .IsUnique()
            .HasDatabaseName("UQ_Enrollments_StudentId_DiplomaId");

        builder.HasOne(e => e.Student)
            .WithMany(u => u.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Diploma)
            .WithMany(d => d.Enrollments)
            .HasForeignKey(e => e.DiplomaId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
