using ExaminationSystem.Application.Features.Atempts.Commands.StartQuiz;
using ExaminationSystem.Application.Features.Atempts.Queries.StartQuizCheck;
using ExaminationSystem.Application.Features.Questions.Query.ShuffleQuestions;
using ExaminationSystem.Application.Features.Quizzes.Queries.CheckQuizId;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Errors;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.Start_Quiz;

public class StartQuizOrchestratorHandler(IMediator mediator) : IRequestHandler<StartQuizOrchestratorCommand, Result<StartQuizResponse>>
{
    private readonly IMediator _mediator = mediator;

    public async Task<Result<StartQuizResponse>> Handle(StartQuizOrchestratorCommand request, CancellationToken cancellationToken)
    {

        // QuizId Found Or Not
        var isQuizFound = await _mediator.Send(new CheckQuizIdQuery(request.QuizId), cancellationToken);
        if (!isQuizFound.Value!.IsFound)
            return Result.Failure<StartQuizResponse>(QuizErrors.NotFound);
        // Attempt limit reached

        var maxAttemptsReach = await _mediator.Send(new AttemptLimitReachedQuery(request.QuizId, request.StudentId, isQuizFound.Value.MaxAttempts), cancellationToken);

        if (!maxAttemptsReach.IsSuccess)
            return Result.Failure<StartQuizResponse>(AttemptErrors.MaxAttempts);

        // Student already has an in-progress attempt for this quiz
        var inProgressAttempt = await _mediator.Send(new InProgressAttemtQuery(request.QuizId, request.StudentId), cancellationToken);
        if (inProgressAttempt.IsSuccess)
            return Result.Success(new StartQuizResponse(inProgressAttempt.Value, request.QuizId, null, null, null, null, null, null));
        // Save attempt And return Id

        var attempt = await _mediator.Send(new StartQuizCommand(request.QuizId, request.StudentId, isQuizFound.Value.DurationMinutes), cancellationToken);
        if (!attempt.IsSuccess)
            return Result.Failure<StartQuizResponse>(attempt.Error);

        // return questions
        var questions = await _mediator.Send(new ShuffleQuestionQuery(request.QuizId), cancellationToken);

        // return all result
        return Result.Success(MapFromStartQuizCommandResponse(questions.Value, attempt.Value));
    }
    private StartQuizResponse MapFromStartQuizCommandResponse(List<ShuffleQuestionResponse> questions, StartQuizCommandResponse attempt)
    {
        return new StartQuizResponse(attempt.AttemptId, attempt.QuizId, attempt.Title, attempt.Instructions, attempt.DurationMinutes, attempt.StartedAt, attempt.Deadline, questions);
    }
}
