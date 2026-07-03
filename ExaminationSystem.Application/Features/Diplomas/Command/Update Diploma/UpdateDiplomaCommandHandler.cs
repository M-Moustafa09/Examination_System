using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Update_Diploma;

public class UpdateDiplomaCommandHandler(IDiplomaRepository _diplomaRepository)
    : IRequestHandler<UpdateDiplomaCommand, Result<UpdateDiplomaCommandResponse>>
{
    public async Task<Result<UpdateDiplomaCommandResponse>> Handle(UpdateDiplomaCommand request,
        CancellationToken cancellationToken)
    {
        var diploma = await _diplomaRepository.GetTable()
            .FirstOrDefaultAsync(d => d.Id == request.Id && !d.IsDeleted, cancellationToken);

        if (diploma is null)
            return Result.Failure<UpdateDiplomaCommandResponse>(DiplomaErrors.NotFound);

        if (!string.IsNullOrEmpty(request.Title))
            diploma.Title = request.Title;
        if (!string.IsNullOrEmpty(request.Description))
            diploma.Description = request.Description;

        _diplomaRepository.Update(diploma);

        var affectedRows = await _diplomaRepository.SaveChangesAsync(cancellationToken);

        if (affectedRows == 0)
            return Result.Failure<UpdateDiplomaCommandResponse>(ResourceErrors.DbError);

        var result = new UpdateDiplomaCommandResponse(diploma.Id, diploma.Title, diploma.Description, diploma.Status,
            diploma.CreatedAt, diploma.UpdatedAt ?? DateTime.UtcNow, diploma.IsDeleted, diploma.DeletedAt ?? DateTime.UtcNow);

        return Result.Success(result);
    }
}