using ExaminationSystem.Application.Features.Atempts.Commands.ValidateAttemptOwnership;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Attempts.Commands.ValidateOwnership;

public class ValidateAttemptOwnershipCommandHandler : IRequestHandler<ValidateAttemptOwnershipCommand, bool>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;

    public ValidateAttemptOwnershipCommandHandler(IGeneralRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<bool> Handle(ValidateAttemptOwnershipCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _attemptRepository.GetTable().Where(a => a.Id == request.AttemptId)
            .FirstOrDefaultAsync(cancellationToken);

        return attempt != null && attempt.StudentId.ToString() == request.UserId;
    }
}