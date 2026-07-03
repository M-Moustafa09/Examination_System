using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Query.GetById;

public record GetQuestionByIdQuery(Guid QuestionId, bool IncludeDeleted = false) : IRequest<Result<QuestionResponse>>;