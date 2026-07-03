using MediatR;
using Microsoft.EntityFrameworkCore;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Application.Common;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttempts;

public class GetAttemptsQueryHandler
    : IRequestHandler<GetAttemptsQuery, Result<PagedResponse<GetAttemptsResponse>>>
{
    private readonly IGeneralRepository<QuizAttempt> _repo;

    public GetAttemptsQueryHandler(IGeneralRepository<QuizAttempt> repo)
    {
        _repo = repo;
    }

    public async Task<Result<PagedResponse<GetAttemptsResponse>>> Handle(
        GetAttemptsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetTable()
            .Include(a => a.Student)
            .Include(a => a.Quiz)
            .AsQueryable();

        if (request.QuizId.HasValue)
            query = query.Where(a => a.QuizId == request.QuizId);

        if (!string.IsNullOrWhiteSpace(request.StudentId))
            query = query.Where(a => a.StudentId == request.StudentId);

        var totalCount = await query.CountAsync(cancellationToken);

        query = request.SortBy?.ToLower() switch
        {
            "createdat" => request.Desc
                ? query.OrderByDescending(a => a.CreatedAt)
                : query.OrderBy(a => a.CreatedAt),

            "score" => request.Desc
                ? query.OrderByDescending(a => a.Score)
                : query.OrderBy(a => a.Score),

            _ => query.OrderByDescending(a => a.Id)
        };

        var data = await query
            .Skip((request.Page - 1) * request.PerPage)
            .Take(request.PerPage)
            .ToListAsync(cancellationToken);

        var items = data.Select(a => new GetAttemptsResponse
        {
            StudentId = a.StudentId,
            StudentName = a.Student?.FullName ?? "Unknown",

            QuizId = a.QuizId,
            QuizTitle = a.Quiz?.Title ?? "Unknown",

            StartTime = a.StartTime,
            SubmittedAt = a.SubmittedAt,

            Status = a.Status,

            Score = a.Score,
            Passed = a.Passed
        }).ToList();

        var response = new PagedResponse<GetAttemptsResponse>
        {
            Data = items,
            Page = request.Page,
            PageSize = request.PerPage,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PerPage)
        };

        return Result.Success(response);
    }
}

