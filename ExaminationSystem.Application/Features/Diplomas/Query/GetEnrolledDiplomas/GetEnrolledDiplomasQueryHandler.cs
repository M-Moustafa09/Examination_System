using ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetailsV2;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas.Query.GetEnrolledDiplomas;

public class GetEnrolledDiplomasQueryHandler(IGeneralRepository<Enrollment> generalRepository, ISender sender) : IRequestHandler<GetEnrolledDiplomasQuery, Result<GetEnrolledDiplomasWithRecentAttemptsResponse>>
{
    private readonly IGeneralRepository<Enrollment> _generalRepository = generalRepository;
    private readonly ISender _sender = sender;

    async Task<Result<GetEnrolledDiplomasWithRecentAttemptsResponse>> IRequestHandler<GetEnrolledDiplomasQuery, Result<GetEnrolledDiplomasWithRecentAttemptsResponse>>.Handle(GetEnrolledDiplomasQuery request, CancellationToken cancellationToken)
    {
        var attemptResult = await _sender.Send(new GetAttemptWithDetailsV2Query(request.StudentId), cancellationToken);

        if (attemptResult.IsFailure)
            return Result.Failure<GetEnrolledDiplomasWithRecentAttemptsResponse>(attemptResult.Error);


        var enrollments = await _generalRepository.GetTable()
            .AsNoTracking()
            .Where(e => e.StudentId == request.StudentId)
            .Select(e => new
            {
                e.DiplomaId,
                DiplomaTitle = e.Diploma.Title,
                QuizCount = e.Diploma.Quizzes.Count
            })
            .ToListAsync(cancellationToken);

        var attempts = attemptResult.Value!;

        var diplomas = enrollments.Select(e => new GetEnrolledDiplomasQueryResponse(
            e.DiplomaId,
            e.DiplomaTitle,
            e.QuizCount,
            attempts.Count(a => a.DiplomaId == e.DiplomaId),
            e.QuizCount > 0
                ? (decimal)attempts.Where(a => a.DiplomaId == e.DiplomaId).Sum(a => a.Passed ? 1 : 0) / e.QuizCount
                : 0m,
            attempts.Any() ? attempts.Max(a => a.SubmittedAt) : default
        )).ToList();

        var response = new GetEnrolledDiplomasWithRecentAttemptsResponse(diplomas, attempts.Select(a =>
            new GetStudentAttemptsResponse(a.AttemptId, a.QuizTitle, a.Score, a.Passed, a.SubmittedAt, a.DurationMinutes)).ToList());

        return Result.Success(response);
    }
}
