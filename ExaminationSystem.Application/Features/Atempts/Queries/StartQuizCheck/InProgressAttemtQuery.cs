using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Queries.StartQuizCheck;

public record InProgressAttemtQuery(Guid QuizId, string UserId) : IRequest<Result<Guid>>;