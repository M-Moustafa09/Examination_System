using ExaminationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Infrastructure.Persistence.Configurations;

public class PasswordResetTokenConfiguration : IEntityTypeConfiguration<PasswordResetToken>
{
    public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(p => p.TokenHash)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.IsUsed).HasDefaultValue(false);
        builder.Property(p => p.IsDeleted).HasDefaultValue(false);

        builder.HasOne(p => p.User)
            .WithMany(u => u.PasswordResetTokens)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
