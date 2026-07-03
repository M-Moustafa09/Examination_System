using MediatR;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using ExaminationSystem.Domain.Abstractions;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptById;

public class GetAttemptByIdHandler
    : IRequestHandler<GetAttemptByIdQuery, Result<GetAttemptByIdResponse>>
{
    private readonly IGeneralRepository<QuizAttempt> _repo;

    public GetAttemptByIdHandler(IGeneralRepository<QuizAttempt> repo)
    {
        _repo = repo;
    }

    public async Task<Result<GetAttemptByIdResponse>> Handle(
        GetAttemptByIdQuery request,
        CancellationToken cancellationToken)
    {
        var attempt = await _repo.GetTable()
            .Include(a => a.Student)
            .Include(a => a.Quiz)
            .Include(a => a.AttemptAnswers)
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);

        if (attempt == null)
        {
            return Result.Failure<GetAttemptByIdResponse>(
                new Error("NotFound", "Attempt not found", 404)
            );
        }

        var response = new GetAttemptByIdResponse
        {
            StudentId = attempt.StudentId,
            StudentName = attempt.Student?.FullName ?? "Unknown",

            QuizId = attempt.QuizId,
            QuizTitle = attempt.Quiz?.Title ?? "Unknown",

            StartTime = attempt.StartTime,
            SubmittedAt = attempt.SubmittedAt,

            Status = attempt.Status,

            Score = attempt.Score,
            Passed = attempt.Passed,

            AttemptAnswers = attempt.AttemptAnswers,

            DurationMinutes = attempt.Quiz?.DurationMinutes ?? 0,
            PassScore = attempt.Quiz?.PassScore ?? 0
        };

        return Result.Success(response);
    }
}



