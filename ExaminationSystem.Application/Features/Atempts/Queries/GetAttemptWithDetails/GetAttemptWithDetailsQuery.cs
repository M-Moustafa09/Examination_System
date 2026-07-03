using ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetails;
using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptWithDetails;

public record GetAttemptWithDetailsQuery(Guid AttemptId) : IRequest<Result<AttemptResponse>>;