using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptResults;

public class GetAttemptResultsQueryHandler(
    IGeneralRepository<QuizAttempt> attemptRepository,
    UserManager<ApplicationUser> userManager)
    : IRequestHandler<GetAttemptResultsQuery, Result<GetAttemptResultsResponse>>
{
    public async Task<Result<GetAttemptResultsResponse>> Handle(
        GetAttemptResultsQuery request,
        CancellationToken cancellationToken)
    {
        var attempt = await attemptRepository.GetTable()
            .AsNoTracking()
            .Where(a => a.Id == request.AttemptId)
            .Include(a => a.Quiz)
            .ThenInclude(q => q.Questions)
            .ThenInclude(q => q.Options)
            .Include(a => a.AttemptAnswers)
            .ThenInclude(aa => aa.SelectedOption)
            .FirstOrDefaultAsync(cancellationToken);

        if (attempt is null)
            return Result.Failure<GetAttemptResultsResponse>(AttemptErrors.NotFound);

        var requester = await userManager.FindByIdAsync(request.RequestingUserId);
        if (requester is null)
            return Result.Failure<GetAttemptResultsResponse>(AttemptErrors.AccessDenied);

        var isAdmin = requester.Role == UserRole.Admin;
        if (!isAdmin && attempt.StudentId != request.RequestingUserId)
            return Result.Failure<GetAttemptResultsResponse>(AttemptErrors.AccessDenied);

        if (attempt.Status == AttemptStatus.InProgress)
            return Result.Failure<GetAttemptResultsResponse>(AttemptErrors.ResultsNotReady);

        var questionsOrdered = attempt.Quiz.Questions
            .OrderBy(q => q.OrderIndex)
            .ThenBy(q => q.Id)
            .ToList();

        var answersByQuestionId = attempt.AttemptAnswers
            .GroupBy(a => a.QuestionId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(x => x.AnsweredAt).First());

        var perQuestion = new List<AttemptQuestionResultItem>();
        foreach (var question in questionsOrdered)
        {
            var correctOption = question.Options.First(o => o.IsCorrect);

            answersByQuestionId.TryGetValue(question.Id, out var answerRow);

            var studentOptionId = answerRow?.SelectedOptionId;
            var studentText = answerRow?.SelectedOption?.Text;
            var isCorrect = answerRow?.IsCorrect ?? false;

            perQuestion.Add(new AttemptQuestionResultItem(
                question.Id,
                question.Text,
                studentOptionId,
                studentText,
                correctOption.Id,
                correctOption.Text,
                isCorrect,
                question.Explanation
            ));
        }

        var totalQuestions = questionsOrdered.Count;
        var correctCount = perQuestion.Count(p => p.IsCorrect);
        var statusLabel = attempt.Status == AttemptStatus.TimedOut ? "timed_out" : "submitted";

        var score = attempt.Score ?? 0;
        var passed = attempt.Passed ?? false;

        return Result.Success(new GetAttemptResultsResponse(
            attempt.Id,
            attempt.QuizId,
            attempt.Quiz.Title,
            score,
            passed,
            statusLabel,
            totalQuestions,
            correctCount,
            perQuestion
        ));
    }
}
