using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Admins;

public record GetAdminStatsQuery : IRequest<Result<GetAdminStatsQueryResponse>>;
