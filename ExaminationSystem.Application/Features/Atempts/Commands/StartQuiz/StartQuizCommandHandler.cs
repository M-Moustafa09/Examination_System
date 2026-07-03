using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Commands.StartQuiz;

public class StartQuizCommandHandler(IGeneralRepository<QuizAttempt> generalRepository) : IRequestHandler<StartQuizCommand, Result<StartQuizCommandResponse>>
{
    private readonly IGeneralRepository<QuizAttempt> _generalRepository = generalRepository;

    public async Task<Result<StartQuizCommandResponse>> Handle(StartQuizCommand request, CancellationToken cancellationToken)
    {
        var attempt = new QuizAttempt
        {
            QuizId = request.QuizId,
            StudentId = request.UserId,
            CreatedAt = DateTime.UtcNow,
            StartTime = DateTime.UtcNow,
            Deadline = DateTime.UtcNow.AddMinutes(request.DurationMinutes),
            Status = Domain.Enums.AttemptStatus.InProgress
        };

        _generalRepository.Add(attempt);
        await _generalRepository.SaveChangesAsync();

        var response = _generalRepository.GetTable()
            .Where(a => a.Id == attempt.Id)
            .Select(
                a => new StartQuizCommandResponse(
                    a.Id,
                    a.QuizId,
                    a.Quiz.Title,
                    a.Quiz.Instructions,
                    a.Quiz.DurationMinutes,
                    a.StartTime,
                    a.Deadline)
                ).SingleOrDefault();

        if (response is null)
            return Result.Failure<StartQuizCommandResponse>(QuizErrors.NotFound);

        return Result.Success(response);
    }
}
