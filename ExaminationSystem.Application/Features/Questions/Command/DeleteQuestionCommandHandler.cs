using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Command;

public class DeleteQuestionCommandHandler(
    IQuizRepository quizRepository,
    IGeneralRepository<Question> questionRepository) : IRequestHandler<DeleteQuestionCommand, Result>
{
    public async Task<Result> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = questionRepository.GetById(request.QuestionId);
        if (question is null || question.IsDeleted)
            return Result.Failure(QuestionErrors.NotFound);

        var quiz = quizRepository.GetById(question.QuizId);
        if (quiz is null || quiz.IsDeleted)
            return Result.Failure(QuestionErrors.QuizNotFound);

        if (quiz.Status == QuizStatus.Published)
            return Result.Failure(QuestionErrors.QuizPublishedConflict);

        question.IsDeleted = true;
        question.DeletedAt = DateTime.UtcNow;
        questionRepository.Update(question);

        var rowsAffected = await questionRepository.SaveChangesAsync();
        if (rowsAffected <= 0)
            return Result.Failure(ResourceErrors.DbError);

        return Result.Success();
    }
}
