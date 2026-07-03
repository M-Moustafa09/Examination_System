using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas.Query.GetQuizData;

public record GetDiplomaQuiznumberQuery(Guid DiplomaId) : IRequest<Result<GetDiplomaQuiznumberResponse>>;

public record GetDiplomaQuiznumberResponse(int QuizNumber);

public class GetDiplomaQuiznumberQueryHandler(IGeneralRepository<Diploma> generalRepository) : IRequestHandler<GetDiplomaQuiznumberQuery, Result<GetDiplomaQuiznumberResponse>>
{
    private readonly IGeneralRepository<Diploma> _generalRepository = generalRepository;

    public async Task<Result<GetDiplomaQuiznumberResponse>> Handle(GetDiplomaQuiznumberQuery request, CancellationToken cancellationToken)
    {
        var response = await _generalRepository.GetTable()
                                                    .Where(q => q.Id == request.DiplomaId)
                                                    .Select(d => new GetDiplomaQuiznumberResponse(d.Quizzes.Count))
                                                    .FirstOrDefaultAsync(cancellationToken);
        if (response is null)
            return Result.Failure<GetDiplomaQuiznumberResponse>(DiplomaErrors.NotFound);

        return Result.Success(response);
    }
}