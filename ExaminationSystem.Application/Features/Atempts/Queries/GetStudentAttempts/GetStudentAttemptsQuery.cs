using ExaminationSystem.Application.Common;
using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetStudentAttempts;

public record GetStudentAttemptsQuery(
    string StudentId,
    int Page = 1,
    int PageSize = 10,
    Guid? AttemptIdFilter = null
) : IRequest<Result<PagedResponse<StudentAttemptSummaryResponse>>>;
