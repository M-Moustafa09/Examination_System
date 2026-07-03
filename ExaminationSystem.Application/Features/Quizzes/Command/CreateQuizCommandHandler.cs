using ExaminationSystem.Application.Features.Diplomas;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Quizzes.Command;

public class CreateQuizCommandHandler(IQuizRepository quizRepository, ISender _sender)
    : IRequestHandler<CreateQuizCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateQuizCommand request, CancellationToken cancellationToken)
    {
        // Check if the diploma exists

        var isDiplomaExist = await _sender.Send(new DiplomaIsFoundQuery(request.DiplomaId), cancellationToken);

        if (!isDiplomaExist.Value)
            return Result.Failure<Guid>(DiplomaErrors.NotFound);

        var quiz = new Quiz
        {
            Title = request.Title,
            DiplomaId = request.DiplomaId,
            Instructions = request.Instructions ?? "",
            DurationMinutes = request.DurationMinutes,
            PassScore = request.PassScore,
            MaxAttempts = request.MaxAttempts,
            Status = QuizStatus.Draft
        };

        quizRepository.Add(quiz);

        var result = await quizRepository.SaveChangesAsync() > 0;

        if (!result)
            return Result.Failure<Guid>(ResourceErrors.DbError);

        return Result.Success(quiz.Id);
    }
}