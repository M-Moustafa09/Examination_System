using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Quizzes.Queries.CheckQuizId;

public class CheckQuizIdQueryHandler(IGeneralRepository<Domain.Entities.Quiz> generalRepository) : IRequestHandler<CheckQuizIdQuery, Result<CheckQuizIdResponse>>
{
    private readonly IGeneralRepository<Domain.Entities.Quiz> _generalRepository = generalRepository;

    public async Task<Result<CheckQuizIdResponse>> Handle(CheckQuizIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _generalRepository.GetTable().FirstOrDefaultAsync(r => r.Id == request.QuizId && r.Status == QuizStatus.Published, cancellationToken);
        return Result.Success(new CheckQuizIdResponse(result is not null, (result is null ? 0 : result.MaxAttempts ?? 0), (result is null ? 0 : result.DurationMinutes)));
    }
}
