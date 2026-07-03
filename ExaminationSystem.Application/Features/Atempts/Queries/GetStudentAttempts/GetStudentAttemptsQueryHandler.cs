using ExaminationSystem.Application.Common;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetStudentAttempts;

public class GetStudentAttemptsQueryHandler : IRequestHandler<GetStudentAttemptsQuery, Result<PagedResponse<StudentAttemptSummaryResponse>>>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;

    public GetStudentAttemptsQueryHandler(IGeneralRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<Result<PagedResponse<StudentAttemptSummaryResponse>>> Handle(
        GetStudentAttemptsQuery request,
        CancellationToken cancellationToken)
    {
        // Start with attempts for the specific student
        var query = _attemptRepository.GetTable()
            .Where(a => a.StudentId.Equals( request.StudentId));

        // Filter by specific attempt ID if provided
        if (request.AttemptIdFilter.HasValue)
        {
            query = query.Where(a => a.Id == request.AttemptIdFilter.Value);
        }

        // Sort by SubmittedAt in descending order (most recent first)
        query = query.OrderByDescending(a => a.SubmittedAt);

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var attempts = await query
            .Include(a => a.Quiz)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        // Map to response DTOs
        var attemptSummaries = attempts.Select(a => new StudentAttemptSummaryResponse(
            AttemptId: a.Id,
            QuizTitle: a.Quiz.Title,
            Score: a.Score,
            Passed: a.Passed,
            Status: a.Status,
            SubmittedAt: a.SubmittedAt
        )).ToList();

        // Build paginated response
        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);
        var pagedResponse = new PagedResponse<StudentAttemptSummaryResponse>
        {
            Data = attemptSummaries,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return Result.Success(pagedResponse);
    }
}
