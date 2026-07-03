using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Questions.Query.ShuffleQuestions;

public class ShuffleQuestionQueryHandler(IGeneralRepository<Question> generalRepository) : IRequestHandler<ShuffleQuestionQuery, Result<List<ShuffleQuestionResponse>>>
{
    private readonly IGeneralRepository<Question> _generalRepository = generalRepository;
    private static readonly Random _rng = new();
    public async Task<Result<List<ShuffleQuestionResponse>>> Handle(ShuffleQuestionQuery request, CancellationToken cancellationToken)
    {
        var questions = await _generalRepository
                .GetTable()
                .Where(q => q.QuizId == request.QuizId)
                .Select(q => new ShuffleQuestionResponse(
                         q.Id,
                         q.Text,
                         q.OrderIndex,
                         q.Options.Select(o => new OptionDto(o.Id, o.Text)).ToList()
                    ))
                .ToListAsync(cancellationToken);

        int n = questions.Count;
        while (n > 1)
        {
            n--;
            int k = _rng.Next(n + 1);
            (questions[k], questions[n]) = (questions[n], questions[k]);
        }

        return Result.Success(questions);
    }
}
