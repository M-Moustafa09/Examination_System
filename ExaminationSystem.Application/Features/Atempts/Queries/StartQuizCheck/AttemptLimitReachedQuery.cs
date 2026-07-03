using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Queries.StartQuizCheck;

public record AttemptLimitReachedQuery(Guid QuizId, string UserId, int MaxAttempts) : IRequest<Result>;