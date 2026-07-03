using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Commands.StartQuiz;

public record StartQuizCommand(Guid QuizId, string UserId, int DurationMinutes) : IRequest<Result<StartQuizCommandResponse>>;