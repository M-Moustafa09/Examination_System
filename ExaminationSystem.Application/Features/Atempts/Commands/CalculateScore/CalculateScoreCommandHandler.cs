using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Attempts.Commands.CalculateScore;

public class CalculateScoreCommandHandler : IRequestHandler<CalculateScoreCommand, CalculateScoreResponse>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;

    public CalculateScoreCommandHandler(IGeneralRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<CalculateScoreResponse> Handle(CalculateScoreCommand request, CancellationToken cancellationToken)
    {
        var attemptQuery = _attemptRepository.GetTable().Where(a => a.Id == request.AttemptId);

        var attempt = await attemptQuery
            .Include(a => a.Quiz)
            .ThenInclude(q => q.Questions)
            .Include(a => a.AttemptAnswers)
            .FirstOrDefaultAsync(cancellationToken);

        if (attempt == null)
            throw new KeyNotFoundException("Attempt not found");

        var totalQuestions = attempt.Quiz.Questions.Count;
        if (totalQuestions == 0)
            return new CalculateScoreResponse(0, false);

        var correctCount = attempt.AttemptAnswers.Count(aa => aa.IsCorrect ?? false);
        var scorePercentage = (int)Math.Round((double)correctCount / totalQuestions * 100);
        var passed = scorePercentage >= request.PassScore;

        return new CalculateScoreResponse(scorePercentage, passed);
    }
}