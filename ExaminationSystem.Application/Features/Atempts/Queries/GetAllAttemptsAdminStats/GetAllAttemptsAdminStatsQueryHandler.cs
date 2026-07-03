using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAllAttemptsAdminStats;

public class GetAllAttemptsAdminStatsQueryHandler(IGeneralRepository<QuizAttempt> generalRepository) : IRequestHandler<GetAllAttemptsAdminStatsQuery, Result<GetAllAttemptsAdminStatsQueryResponse>>
{
    private readonly IGeneralRepository<QuizAttempt> _generalRepository = generalRepository;

    public async Task<Result<GetAllAttemptsAdminStatsQueryResponse>> Handle(GetAllAttemptsAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var studentids = await _generalRepository.GetTable().Where(qa => DateOnly.FromDateTime(qa.StartTime) == DateOnly.FromDateTime(DateTime.UtcNow) && !qa.IsDeleted).Select(qa => qa.StudentId).ToListAsync(cancellationToken);

        var totalAttemps = await _generalRepository.GetTable().Where(qa => !qa.IsDeleted).Select(qa => qa.Passed ?? false).ToListAsync(cancellationToken);

        return Result.Success(new GetAllAttemptsAdminStatsQueryResponse(studentids, totalAttemps));
    }
}
