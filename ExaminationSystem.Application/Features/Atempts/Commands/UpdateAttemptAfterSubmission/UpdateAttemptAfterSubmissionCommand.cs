using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Attempts.Commands.UpdateAttemptAfterSubmission;

public record UpdateAttemptAfterSubmissionCommand(Guid AttemptId, int Score, bool Passed, bool IsTimedOut)
    : IRequest<Result<bool>>;