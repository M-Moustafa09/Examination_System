using ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptWithDetails;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetails;

public class GetAttemptWithDetailsQueryHandler : IRequestHandler<GetAttemptWithDetailsQuery, Result<AttemptResponse>>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;

    public GetAttemptWithDetailsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<Result<AttemptResponse>> Handle(GetAttemptWithDetailsQuery request,
        CancellationToken cancellationToken)
    {
        // الـ Get يعيد IQueryable، نستخدم Include ثم FirstOrDefaultAsync
        var query = _attemptRepository.GetTable().Where(a => a.Id == request.AttemptId);
        var attempt = await query
            .Include(a => a.Quiz)
            .ThenInclude(q => q.Questions)
            .ThenInclude(q => q.Options)
            .Include(a => a.AttemptAnswers)
            .Include(a => a.Student)
            .FirstOrDefaultAsync(cancellationToken);


        var attemptresponce = new AttemptResponse(
            attempt.Id,
            attempt.StudentId,
            attempt.Student.FullName,
            //"abo salah",
            attempt.QuizId,
            attempt.Quiz.Title,
            attempt.StartTime,
            attempt.SubmittedAt,
            attempt.Status,
            attempt.Score,
            attempt.Passed,
            attempt.AttemptAnswers,
            attempt.Quiz.DurationMinutes,
            attempt.Quiz.PassScore
        );

        return Result.Success(attemptresponce);
    }
}