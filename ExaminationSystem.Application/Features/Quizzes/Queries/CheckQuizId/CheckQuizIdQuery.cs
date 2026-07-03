using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Quizzes.Queries.CheckQuizId;

public record CheckQuizIdQuery(Guid QuizId) : IRequest<Result<CheckQuizIdResponse>>;