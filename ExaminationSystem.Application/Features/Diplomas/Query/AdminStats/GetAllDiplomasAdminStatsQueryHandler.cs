using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas.Query.AdminStats;

public class GetAllDiplomasAdminStatsQueryHandler(IGeneralRepository<Diploma> generalRepository) : IRequestHandler<GetAllDiplomasAdminStatsQuery, Result<GetAllDiplomasAdminStatsQueryResponse>>
{
    private readonly IGeneralRepository<Diploma> _generalRepository = generalRepository;

    public async Task<Result<GetAllDiplomasAdminStatsQueryResponse>> Handle(GetAllDiplomasAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var result = await _generalRepository.GetTable().CountAsync(cancellationToken);
        return Result.Success(new GetAllDiplomasAdminStatsQueryResponse(result));
    }
}
