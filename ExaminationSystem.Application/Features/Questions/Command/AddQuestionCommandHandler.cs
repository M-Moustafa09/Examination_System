using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Command;

public class AddQuestionCommandHandler(
    IQuizRepository quizRepository,
    IGeneralRepository<Question> questionRepository) : IRequestHandler<AddQuestionCommand, Result<int>>
{
    public async Task<Result<int>> Handle(AddQuestionCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
        //var quiz = quizRepository.GetById(request.QuizId);
        //if (quiz is null || quiz.IsDeleted)
        //    return Result.Failure<int>(QuestionErrors.QuizNotFound);

        //if (quiz.Status == QuizStatus.Published)
        //    return Result.Failure<int>(QuestionErrors.QuizPublishedConflict);

        //var question = new Question
        //{
        //    QuizId = request.QuizId,
        //    Text = request.Text,
        //    Explanation = request.Explanation ?? string.Empty,
        //    OrderIndex = request.OrderIndex,
        //    Options = request.Options.Select(option => new
        //    {
        //        Text = option.Text,
        //        IsCorrect = option.IsCorrect
        //    }).ToList()
        //};

        //questionRepository.Add(question);

        //var rowsAffected = await questionRepository.SaveChangesAsync();
        //if (rowsAffected <= 0)
        //    return Result.Failure<int>(ResourceErrors.DbError);

        //return Result.Success(question.Id);
    }
}
