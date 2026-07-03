using ExaminationSystem.Domain.Commons;
using ExaminationSystem.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExaminationSystem.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Diploma> Diplomas { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<AttemptAnswer> AttemptAnswers { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizAttempt> QuizAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        foreach (var entityEntry in entries)
            if (entityEntry.State == EntityState.Added)
                entityEntry.Entity.CreatedAt = DateTime.UtcNow;
            else if (entityEntry.State == EntityState.Modified)

                entityEntry.Entity.UpdatedAt = DateTime.UtcNow;
            else
                entityEntry.Entity.DeletedAt = DateTime.UtcNow;

        return base.SaveChangesAsync(cancellationToken);
    }
}
