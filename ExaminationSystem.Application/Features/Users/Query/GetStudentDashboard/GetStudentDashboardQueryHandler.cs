using ExaminationSystem.Application.Features.Diplomas.Query.GetEnrolledDiplomas;
using ExaminationSystem.Application.Features.Users.Query.GetStudent;
using ExaminationSystem.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Application.Features.Users.Query.GetStudentDashboard;

public class GetStudentDashboardQueryHandler(ISender sender, IMemoryCache memoryCache) : IRequestHandler<GetStudentDashboardQuery, Result<GetStudentDashboardResponse>>
{
    private readonly ISender _sender = sender;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private string perfix = "StudendCache ";

    public async Task<Result<GetStudentDashboardResponse>> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
    {
        var chche = _memoryCache.TryGetValue($"{perfix}:{request.StudentId}", out GetStudentDashboardResponse? result);

        if (chche && result is not null)
            return Result.Success(result);

        var student = await _sender.Send(new GetStudentQuery(request.StudentId), cancellationToken);
        if (student.IsFailure)
            return Result.Failure<GetStudentDashboardResponse>(student.Error);


        var enrolledDiplomasWithRecentAttempts = await _sender.Send(new GetEnrolledDiplomasQuery(request.StudentId), cancellationToken);
        if (enrolledDiplomasWithRecentAttempts.IsFailure)
            return Result.Failure<GetStudentDashboardResponse>(enrolledDiplomasWithRecentAttempts.Error);

        var totalQuizzesTaken = enrolledDiplomasWithRecentAttempts.Value!.GetEnrolledDiplomas.Sum(q => q.CompletedQuizzes);
        var totalScore = totalQuizzesTaken > 0
            ? enrolledDiplomasWithRecentAttempts.Value!.AttemptsResponse.Sum(q => q.Score) / totalQuizzesTaken
            : 0;

        var passedCount = enrolledDiplomasWithRecentAttempts.Value!.AttemptsResponse.Count(q => q.Passed);
        var passRate = totalQuizzesTaken > 0
            ? (decimal)passedCount / totalQuizzesTaken
            : 0m; var totalTimeSpentMinutes = enrolledDiplomasWithRecentAttempts.Value!.AttemptsResponse.Sum(q => q.TimeSpentMinutes);


        var overallStats = new GetStudentDashboardOverallStatsResponse(
               totalQuizzesTaken,
               totalScore,
               passRate,
               totalTimeSpentMinutes
            );

        var response = new GetStudentDashboardResponse(
                new GetStudentDashboardStudentResponse(request.StudentId, student.Value!.FullName, student.Value!.Email)
                , enrolledDiplomasWithRecentAttempts.Value.GetEnrolledDiplomas.Select(e => new GetStudentDashboardEnrolledDiplomasResponse(
                        e.DiplomaId,
                        e.Title,
                        e.QuizCount,
                        e.CompletedQuizzes,
                        e.ProgressPercentage,
                        e.LastActivityAt
                    )).ToList()
                ,
                    enrolledDiplomasWithRecentAttempts.Value.AttemptsResponse.Select(a => new GetStudentDashboardRecentAttemptsResponse(
                            a.AttemptId,
                            a.QuizTitle,
                            a.Score,
                            a.Passed,
                            a.SubmittedAt
                        )).ToList()
                , overallStats);

        _memoryCache.Set($"{perfix}:{request.StudentId}", response, DateTimeOffset.FromUnixTimeSeconds(60));
        return Result.Success(response);
    }
}
