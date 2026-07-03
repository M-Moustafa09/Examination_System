using ExaminationSystem.Application.Features.Atempts.Commands.CheckAttemptInProgress;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Attempts.Commands.CheckProgress;

public class CheckAttemptInProgressCommandHandler : IRequestHandler<CheckAttemptInProgressCommand, bool>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;

    public CheckAttemptInProgressCommandHandler(IGeneralRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<bool> Handle(CheckAttemptInProgressCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _attemptRepository.GetTable().Where(a => a.Id == request.AttemptId)
            .FirstOrDefaultAsync(cancellationToken);

        return attempt != null && attempt.Status == AttemptStatus.InProgress;
    }
}