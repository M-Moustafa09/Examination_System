namespace ExaminationSystem.Application.Features.Admins;

public record GetAdminStatsQueryResponse(int TotalUsers, int ActiveUsersToday, int TotalDiplomas, int TotalQuizzes, int TotalAttempts, decimal AvgPassRate);
