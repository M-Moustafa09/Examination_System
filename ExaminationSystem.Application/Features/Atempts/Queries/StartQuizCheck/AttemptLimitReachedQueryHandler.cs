using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.StartQuizCheck;

public class AttemptLimitReachedQueryHandler(IGeneralRepository<QuizAttempt> generalRepository) : IRequestHandler<AttemptLimitReachedQuery, Result>
{
    private readonly IGeneralRepository<QuizAttempt> _generalRepository = generalRepository;

    public async Task<Result> Handle(AttemptLimitReachedQuery request, CancellationToken cancellationToken)
    {
        var result = await _generalRepository.GetTable().CountAsync(a => a.QuizId == request.QuizId && a.StudentId == request.UserId, cancellationToken);
        return result > request.MaxAttempts
            ? Result.Failure(AttemptErrors.MaxAttempts)
            : Result.Success();
    }
}
