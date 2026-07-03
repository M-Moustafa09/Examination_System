using MediatR;

namespace ExaminationSystem.Application.Features.Admin.Queries.GetAdminStats;

public record GetAdminStatsQuery() : IRequest<AdminStatsDto>;
