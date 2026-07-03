using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Command;

public record DeleteQuestionCommand(int QuestionId) : IRequest<Result>;
