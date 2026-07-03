using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace ExaminationSystem.Application.Features.Admin.Queries.GetAdminStats;

public class GetAdminStatsQueryHandler : IRequestHandler<GetAdminStatsQuery, AdminStatsDto>
{
    private readonly IGeneralRepository<ApplicationUser> _userRepository;
    private readonly IGeneralRepository<Quiz> _quizRepository;
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;
    private readonly IMemoryCache _cache;

    public GetAdminStatsQueryHandler(
        IGeneralRepository<ApplicationUser> userRepository,
        IGeneralRepository<Quiz> quizRepository,
        IGeneralRepository<QuizAttempt> attemptRepository,
        IMemoryCache cache)
    {
        _userRepository = userRepository;
        _quizRepository = quizRepository;
        _attemptRepository = attemptRepository;
        _cache = cache;
    }

    public async Task<AdminStatsDto> Handle(GetAdminStatsQuery request, CancellationToken cancellationToken)
    {
        const string cacheKey = "admin_stats";
        if (_cache.TryGetValue(cacheKey, out AdminStatsDto cached))
            return cached;

        var totalUsers = await _userRepository.GetTable().CountAsync(cancellationToken);

        var utcToday = DateTime.UtcNow.Date;
        var nextUtcDay = utcToday.AddDays(1);
        var activeUsersToday = await _userRepository.GetTable()
            .Where(u => u.LastLoginAt.HasValue && u.LastLoginAt.Value >= utcToday && u.LastLoginAt.Value < nextUtcDay)
            .CountAsync(cancellationToken);

        var totalQuizzes = await _quizRepository.GetTable().CountAsync(cancellationToken);

        var totalAttempts = await _attemptRepository.GetTable().CountAsync(cancellationToken);

        decimal avgPassRate = 0m;
        if (totalAttempts > 0)
        {
            var passedCount = await _attemptRepository.GetTable().Where(a => a.Passed == true).CountAsync(cancellationToken);
            avgPassRate = Math.Round((decimal)passedCount / totalAttempts * 100, 2);
        }

        var dto = new AdminStatsDto(totalUsers, activeUsersToday, totalQuizzes, totalAttempts, avgPassRate);

        _cache.Set(cacheKey, dto, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        });

        return dto;
    }
}
