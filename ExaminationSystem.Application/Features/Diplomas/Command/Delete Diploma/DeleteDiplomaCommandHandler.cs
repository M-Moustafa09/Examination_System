using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Delete_Diploma;

public class DeleteDiplomaCommandHandler(IDiplomaRepository _diplomaRepository)
    : IRequestHandler<DeleteDiplomaCommand, Result>
{
    public async Task<Result> Handle(DeleteDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetTable()
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (diploma is null)
            return Result.Failure<DeleteDiplomaCommand>(DiplomaErrors.NotFound);

        diploma.IsDeleted = true;
        diploma.DeletedAt = DateTime.Now;
        
        return Result.Success();
    }
}