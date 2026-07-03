using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Query.ShuffleQuestions;

public record ShuffleQuestionQuery(Guid QuizId) : IRequest<Result<List<ShuffleQuestionResponse>>>;
