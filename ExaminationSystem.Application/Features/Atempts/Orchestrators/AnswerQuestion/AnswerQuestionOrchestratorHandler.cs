using ExaminationSystem.Application.Features.Attempts.Orchestrators.SubmitQuiz;
using ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptWithDetails;
using ExaminationSystem.Application.Features.Options;
using ExaminationSystem.Application.Features.Questions.Query.GetById;
using ExaminationSystem.Application.Interfaces;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.AnswerQuestion;

public class AnswerQuestionOrchestratorHandler(ISender _sender, IGeneralRepository<AttemptAnswer> _attemptAnswerRepository, ICurrentUserService _currentUserService)
    : IRequestHandler<AnswerQuestionOrchestrator, Result<AnswerQuestionResponse>>
{
    public async Task<Result<AnswerQuestionResponse>> Handle(AnswerQuestionOrchestrator request, CancellationToken cancellationToken)
    {
        // 1. Load attempt — 404 if not found
        var attemptResult = await _sender.Send(new GetAttemptWithDetailsQuery(request.AttemptId), cancellationToken);

        if (attemptResult.IsFailure)
            return Result.Failure<AnswerQuestionResponse>(attemptResult.Error);


        var attempt = attemptResult.Value!;

        // 2. Ownership check — requester must own the attempt
        if (attempt.StudentId != _currentUserService.Id)
            return Result.Failure<AnswerQuestionResponse>(QuestionErrors.AnswerForbidden);

        // 3. Attempt must be in_progress
        if (attempt.Status != AttemptStatus.InProgress)
            return Result.Failure<AnswerQuestionResponse>(AttemptErrors.NoInProgresAttempt);


        // 4. Deadline check on every submission
        var deadline = attempt.StartTime.Add(TimeSpan.FromMinutes(attempt.DurationMinutes));
        var now = DateTime.UtcNow;

        if (now > deadline)
        {
            await _sender.Send(new SubmitQuizCommand(attempt.Id, _currentUserService.Id), cancellationToken);
            return Result.Failure<AnswerQuestionResponse>(AttemptErrors.DeadlinePassed);
        }


        // 5. questionId must belong to this quiz
        var questionResult = await _sender.Send(new GetQuestionByIdQuery(request.QuestionId), cancellationToken);

        if (questionResult.IsFailure)
            return Result.Failure<AnswerQuestionResponse>(questionResult.Error);


        // 6. check if options belongs to the question and determine correctness

        var optionResult = await _sender.Send(new GetOptionQuery(request.SelectedOptionId), cancellationToken);

        if (optionResult.IsFailure)
            return Result.Failure<AnswerQuestionResponse>(optionResult.Error);

        var option = optionResult.Value!;

        var answeredAt = now;

        var answer = new AttemptAnswer
        {
            AttemptId = attempt.Id,
            QuestionId = request.QuestionId,
            SelectedOptionId = request.SelectedOptionId,
            AnsweredAt = answeredAt,
            IsCorrect = option.IsCorrect,
        };


        _attemptAnswerRepository.Add(answer);

        await _attemptAnswerRepository.SaveChangesAsync();

        var response = new AnswerQuestionResponse(
            QuestionId: request.QuestionId,
            SelectedOptionId: request.SelectedOptionId,
            AnsweredAt: answeredAt
        );

        return Result.Success(response);
    }
}
