using ExaminationSystem.Application.Features.Atempts.Commands.CheckAttemptInProgress;
using ExaminationSystem.Application.Features.Atempts.Commands.ValidateAttemptOwnership;
using ExaminationSystem.Application.Features.Attempts.Commands.CalculateScore;
using ExaminationSystem.Application.Features.Attempts.Commands.UpdateAttemptAfterSubmission;
using ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptWithDetails;
using MediatR;

namespace ExaminationSystem.Application.Features.Attempts.Orchestrators.SubmitQuiz;






public class SubmitQuizCommandHandler : IRequestHandler<SubmitQuizCommand, SubmitQuizResult>
{
    private readonly IMediator _mediator;

    public SubmitQuizCommandHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<SubmitQuizResult> Handle(SubmitQuizCommand request, CancellationToken cancellationToken)
    {
        var attempt_result = await _mediator.Send(new GetAttemptWithDetailsQuery(request.AttemptId));
        var attempt = attempt_result.Value;
        if (attempt_result == null)
            throw new KeyNotFoundException($"Attempt with ID {request.AttemptId} not found.");

        var isOwner = await _mediator.Send(new ValidateAttemptOwnershipCommand(request.AttemptId, request.UserId.ToString()));
        if (!isOwner)
            throw new UnauthorizedAccessException("You are not allowed to submit this attempt.");

        var isInProgress = await _mediator.Send(new CheckAttemptInProgressCommand(request.AttemptId));
        if (!isInProgress)
            throw new InvalidOperationException("Attempt is already submitted or timed out.");

        var deadline = attempt.StartTime.AddMinutes(attempt.DurationMinutes);
        var isTimedOut = DateTime.UtcNow > deadline;


        var scoreResponse = await _mediator.Send(new CalculateScoreCommand
        (request.AttemptId,
            attempt.PassScore
        ));

        await _mediator.Send(new UpdateAttemptAfterSubmissionCommand
        (
            request.AttemptId,
            scoreResponse.Score,
            scoreResponse.Passed,
            isTimedOut
        ));

        return new SubmitQuizResult
        (
            request.AttemptId,
            scoreResponse.Score,
            scoreResponse.Passed,
            isTimedOut ? "timed_out" : "submitted"
        );
    }

}
