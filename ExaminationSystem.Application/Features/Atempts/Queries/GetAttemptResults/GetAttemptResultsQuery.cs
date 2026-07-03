using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptResults;

public record GetAttemptResultsQuery(Guid AttemptId, string RequestingUserId)
    : IRequest<Result<GetAttemptResultsResponse>>;
