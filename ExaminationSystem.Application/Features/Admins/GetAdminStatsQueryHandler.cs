using ExaminationSystem.Application.Features.Atempts.Queries.GetAllAttemptsAdminStats;
using ExaminationSystem.Application.Features.Diplomas.Query.AdminStats;
using ExaminationSystem.Application.Features.Quizzes.Queries.GetAllQuizzesAdminStats;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace ExaminationSystem.Application.Features.Admins;

public class GetAdminStatsQueryHandler(IGeneralRepository<ApplicationUser> generalRepository, ISender sender, IMemoryCache memoryCache) : IRequestHandler<GetAdminStatsQuery, Result<GetAdminStatsQueryResponse>>
{
    private readonly IGeneralRepository<ApplicationUser> _generalRepository = generalRepository;
    private readonly ISender _sender = sender;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private string perfix = "AdminStats";

    async Task<Result<GetAdminStatsQueryResponse>> IRequestHandler<GetAdminStatsQuery, Result<GetAdminStatsQueryResponse>>.Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {
        var chche = _memoryCache.TryGetValue(perfix, out GetAdminStatsQueryResponse? result);

        if (chche && result is not null)
            return Result.Success(result);

        var totalUsers = await _generalRepository.GetTable().Where(u => !u.IsDeleted).Select(u => u.Id).ToListAsync(cancellationToken);

        var totalAttempts = await _sender.Send(new GetAllAttemptsAdminStatsQuery(), cancellationToken);

        var activeUsersToday = totalUsers.Count(u => totalAttempts.Value.StudentsIds.Contains(u));

        var totalAttempsCount = totalAttempts.Value.TotalAttemps.Count();
        var totalPassedAttempsCount = totalAttempts.Value.TotalAttemps.Count(at => at);
        var avgPassRate = totalAttempsCount == 0 ? 0 : (decimal)totalPassedAttempsCount / totalAttempsCount * 100;

        var totalQuizzes = await _sender.Send(new GetAllQuizzesAdminStatsQuery(), cancellationToken);

        var totalDiplomas = await _sender.Send(new GetAllDiplomasAdminStatsQuery(), cancellationToken);

        var response = new GetAdminStatsQueryResponse(totalUsers.Count, activeUsersToday, totalDiplomas.Value.NumberOfDiplomas, totalQuizzes.Value.NumberOfQuizzes, totalAttempsCount, avgPassRate);

        _memoryCache.Set(perfix, response, DateTimeOffset.FromUnixTimeSeconds(60 * 5));

        return Result.Success(response);
    }
}
