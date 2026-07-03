using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Commands.ValidateAttemptOwnership;

public record ValidateAttemptOwnershipCommand(Guid AttemptId, string UserId) : IRequest<bool>;