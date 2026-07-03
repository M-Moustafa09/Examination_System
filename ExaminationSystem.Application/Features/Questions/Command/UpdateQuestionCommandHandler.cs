using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Command;

public class UpdateQuestionCommandHandler(
    IQuizRepository quizRepository,
    IGeneralRepository<Question> questionRepository,
    IGeneralRepository<Option> answerOptionRepository) : IRequestHandler<UpdateQuestionCommand, Result>
{
    public async Task<Result> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = questionRepository.GetById(request.QuestionId);
        if (question is null || question.IsDeleted)
            return Result.Failure(QuestionErrors.NotFound);

        var quiz = quizRepository.GetById(question.QuizId);
        if (quiz is null || quiz.IsDeleted)
            return Result.Failure(QuestionErrors.QuizNotFound);

        if (quiz.Status == QuizStatus.Published)
            return Result.Failure(QuestionErrors.QuizPublishedConflict);

        question.Text = request.Text;
        question.Explanation = request.Explanation ?? string.Empty;
        question.OrderIndex = request.OrderIndex;
        questionRepository.Update(question);

        var existingOptions = answerOptionRepository.GetTable()
            .Where(option => option.QuestionId == question.Id)
            .ToList();

        foreach (var option in existingOptions)
            answerOptionRepository.Delete(option);

        foreach (var option in request.Options)
        {
            answerOptionRepository.Add(new Option
            {
                QuestionId = question.Id,
                Text = option.Text,
                IsCorrect = option.IsCorrect
            });
        }

        var rowsAffected = await answerOptionRepository.SaveChangesAsync();
        if (rowsAffected <= 0)
            return Result.Failure(ResourceErrors.DbError);

        return Result.Success();
    }
}
