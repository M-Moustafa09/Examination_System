using ExaminationSystem.Application.Features.Attempts.Commands.UpdateAttemptAfterSubmission;
using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Attempts.Commands.UpdateSubmission;

public class
    UpdateAttemptAfterSubmissionCommandHandler : IRequestHandler<UpdateAttemptAfterSubmissionCommand, Result<bool>>
{
    private readonly IGeneralRepository<QuizAttempt> _attemptRepository;

    public UpdateAttemptAfterSubmissionCommandHandler(IGeneralRepository<QuizAttempt> attemptRepository)
    {
        _attemptRepository = attemptRepository;
    }

    public async Task<Result<bool>> Handle(UpdateAttemptAfterSubmissionCommand request,
        CancellationToken cancellationToken)
    {
        var attempt = await _attemptRepository.GetTable().Where(a => a.Id == request.AttemptId)
            .FirstOrDefaultAsync(cancellationToken);

        if (attempt == null) throw new KeyNotFoundException($"Attempt with ID {request.AttemptId} not found");

        attempt.Score = request.Score;
        attempt.Passed = request.Passed;
        attempt.SubmittedAt = DateTime.UtcNow;
        attempt.Status = request.IsTimedOut ? AttemptStatus.TimedOut : AttemptStatus.Submitted;

        _attemptRepository.Update(attempt);
        // await _attemptRepository.SaveChangesAsync();  // تم إضافة الحفظ

        return Result.Success(true);
    }
}