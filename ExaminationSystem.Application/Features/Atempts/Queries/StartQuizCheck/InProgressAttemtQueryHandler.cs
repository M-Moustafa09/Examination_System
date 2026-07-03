using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.StartQuizCheck;

public class InProgressAttemtQueryHandler(IGeneralRepository<QuizAttempt> generalRepository) : IRequestHandler<InProgressAttemtQuery, Result<Guid>>

{
    private readonly IGeneralRepository<QuizAttempt> _generalRepository = generalRepository;

    public async Task<Result<Guid>> Handle(InProgressAttemtQuery request, CancellationToken cancellationToken)
    {
        var inProgressAttemt = await _generalRepository.GetTable().FirstOrDefaultAsync(a => a.QuizId == request.QuizId && a.StudentId == request.UserId && a.Status == AttemptStatus.InProgress, cancellationToken);

        if (inProgressAttemt is not null)
            return Result.Success(inProgressAttemt.Id);

        return Result.Failure<Guid>(AttemptErrors.NoInProgresAttempt);
    }
}
