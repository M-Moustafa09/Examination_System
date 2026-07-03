using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Quizzes.Queries.GetAllQuizzesAdminStats;

public class GetAllQuizzesAdminStatsQueryHandler(IGeneralRepository<Quiz> generalRepository) : IRequestHandler<GetAllQuizzesAdminStatsQuery, Result<GetAllQuizzesAdminStatsQueryResponse>>
{
    private readonly IGeneralRepository<Quiz> _generalRepository = generalRepository;

    public async Task<Result<GetAllQuizzesAdminStatsQueryResponse>> Handle(GetAllQuizzesAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var total = await _generalRepository.GetTable().Where(q => !q.IsDeleted).CountAsync(cancellationToken);

        return Result.Success(new GetAllQuizzesAdminStatsQueryResponse(total));
    }
}
