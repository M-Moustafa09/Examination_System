using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Query.GetById;

public record GetQuestionByIdQueryHandler(IGeneralRepository<Question> _questionRepository) : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponse>>
{
    public async Task<Result<QuestionResponse>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = _questionRepository.GetById(request.QuestionId);

        if (question is null || question.IsDeleted)
            return Result.Failure<QuestionResponse>(QuestionErrors.NotFound);


        var response = new QuestionResponse
        (
           question.Id,
           question.QuizId,
           question.Text,
           question.Explanation,
           question.OrderIndex
        );

        return Result.Success(response);
    }
}