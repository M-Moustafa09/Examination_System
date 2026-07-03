using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Commands.CheckAttemptInProgress;

public record CheckAttemptInProgressCommand(Guid AttemptId) : IRequest<bool>;