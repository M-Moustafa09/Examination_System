namespace ExaminationSystem.Application.Features.Admin.Queries.GetAdminStats;

public record AdminStatsDto(
    int TotalUsers,
    int ActiveUsersToday,
    int TotalQuizzes,
    int TotalAttempts,
    decimal AvgPassRate
);
