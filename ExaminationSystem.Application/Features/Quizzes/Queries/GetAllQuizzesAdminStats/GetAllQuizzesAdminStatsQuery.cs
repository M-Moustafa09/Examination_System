using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Quizzes.Queries.GetAllQuizzesAdminStats;

public record GetAllQuizzesAdminStatsQuery() : IRequest<Result<GetAllQuizzesAdminStatsQueryResponse>>;

public record GetAllQuizzesAdminStatsQueryResponse(int NumberOfQuizzes);