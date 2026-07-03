using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetailsV2;

public class GetAttemptWithDetailsV2QueryHandler(IGeneralRepository<QuizAttempt> generalRepository) : IRequestHandler<GetAttemptWithDetailsV2Query, Result<List<GetAttemptWithDetailsV2Response>>>
{
    private readonly IGeneralRepository<QuizAttempt> _generalRepository = generalRepository;

    public async Task<Result<List<GetAttemptWithDetailsV2Response>>> Handle(GetAttemptWithDetailsV2Query request, CancellationToken cancellationToken)
    {
        var response = await _generalRepository.GetTable()
                    .Where(qa => qa.StudentId == request.StudentId && qa.Status == AttemptStatus.Submitted && !qa.IsDeleted)
                    .Select(qa => new GetAttemptWithDetailsV2Response(
                            qa.Id,
                            qa.Quiz.Title,
                            qa.Quiz.DiplomaId,
                            qa.Quiz.Id,
                            qa.Score ?? 0,
                            qa.Score >= qa.Quiz.PassScore,
                            qa.SubmittedAt ?? DateTime.UtcNow,
                            qa.Quiz.DurationMinutes
                        ))
                    .ToListAsync(cancellationToken);

        return Result.Success(response);
    }
}
