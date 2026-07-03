using ExaminationSystem.Application.Features.Diplomas;
using ExaminationSystem.Application.Features.Enrollments.Diplomas;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Quizzes;

public class GetDiplomaQuizzesQueryHandler(IGeneralRepository<Quiz> generalRepository, IMediator mediator)
    : IRequestHandler<GetDiplomaQuizzesQuery, Result<List<DiplomaQuizzesQueryResponse>>>
{
    private readonly IGeneralRepository<Quiz> _generalRepository = generalRepository;
    private readonly IMediator _mediator = mediator;

    public async Task<Result<List<DiplomaQuizzesQueryResponse>>> Handle(GetDiplomaQuizzesQuery request,
        CancellationToken cancellationToken)
    {
        var isDiplomaFound = await _mediator.Send(new DiplomaIsFoundQuery(request.DiplomaId), cancellationToken);
        if (!isDiplomaFound.Value)
            return Result.Failure<List<DiplomaQuizzesQueryResponse>>(DiplomaErrors.NotFound);

        var isEnrollment = await _mediator.Send(new GetDiplomaEnrollmentQuery(request.DiplomaId, request.StudentId),
            cancellationToken);
        if (!isEnrollment.Value)
            return Result.Failure<List<DiplomaQuizzesQueryResponse>>(DiplomaErrors.NotEnrolled);

        var result = await _generalRepository.GetTable().Where(q => q.DiplomaId == request.DiplomaId
                                                                    && q.Status == QuizStatus.Published)
            .Select(q => new DiplomaQuizzesQueryResponse(
                q.Id, q.Title, q.DurationMinutes, q.PassScore, q.MaxAttempts, q.QuizAttempts.Count, q.PassScore, q.Status
            )).ToListAsync(cancellationToken);
        return Result.Success(result);
    }
}