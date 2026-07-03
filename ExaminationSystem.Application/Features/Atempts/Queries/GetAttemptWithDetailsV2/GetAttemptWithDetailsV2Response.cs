namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetailsV2;

public record GetAttemptWithDetailsV2Response(
        Guid AttemptId, string QuizTitle, Guid DiplomaId, Guid QuizId, decimal Score, bool Passed, DateTime SubmittedAt,
        int DurationMinutes
    );
/*
   Guid QuizId,
    string Quiz_name,
    DateTime StartTime,
    DateTime? SubmittedAt,
    AttemptStatus Status,
    decimal? Score,
    bool? Passed,
    ICollection<AttemptAnswer> AttemptAnswers,
    int DurationMinutes,
    decimal PassScore
 * 
 * 
 * 
 */