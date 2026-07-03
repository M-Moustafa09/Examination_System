namespace ExaminationSystem.Application.Features.Diplomas.Query.GetEnrolledDiplomas;

public record GetEnrolledDiplomasQueryResponse(Guid DiplomaId, string Title, int QuizCount,
                    int CompletedQuizzes, decimal ProgressPercentage, DateTime LastActivityAt);
public record GetStudentAttemptsResponse(Guid AttemptId, string QuizTitle, decimal Score, bool Passed, DateTime SubmittedAt, int TimeSpentMinutes);
public record GetEnrolledDiplomasWithRecentAttemptsResponse(List<GetEnrolledDiplomasQueryResponse> GetEnrolledDiplomas, List<GetStudentAttemptsResponse> AttemptsResponse);