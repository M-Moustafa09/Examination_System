using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Orchestrators.Start_Quiz;

public record StartQuizOrchestratorCommand(Guid QuizId, string StudentId) : IRequest<Result<StartQuizResponse>>;