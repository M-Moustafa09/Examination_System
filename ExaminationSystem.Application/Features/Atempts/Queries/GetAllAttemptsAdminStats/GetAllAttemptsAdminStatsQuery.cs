using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAllAttemptsAdminStats;

public record GetAllAttemptsAdminStatsQuery() : IRequest<Result<GetAllAttemptsAdminStatsQueryResponse>>;
public record GetAllAttemptsAdminStatsQueryResponse(List<string> StudentsIds, List<bool> TotalAttemps);
