using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Query.AdminStats;

public record GetAllDiplomasAdminStatsQuery : IRequest<Result<GetAllDiplomasAdminStatsQueryResponse>>;

public record GetAllDiplomasAdminStatsQueryResponse(int NumberOfDiplomas);