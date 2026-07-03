using MediatR;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Application.Common;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttempts;

public class GetAttemptsQuery : IRequest<Result<PagedResponse<GetAttemptsResponse>>>
{
    public Guid? QuizId { get; set; }
    public string? StudentId { get; set; }

    public int Page { get; set; } = 1;
    public int PerPage { get; set; } = 20;

    public string? SortBy { get; set; }
    public bool Desc { get; set; } = false;
}
