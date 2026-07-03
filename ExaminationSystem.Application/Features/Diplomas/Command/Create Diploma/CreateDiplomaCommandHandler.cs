using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Create_Diploma;

public class CreateDiplomaCommandHandler(IDiplomaRepository _diplomaRepository) : IRequestHandler<CreateDiplomaCommand, Result<CreateDiplomaCommandResponse>>
{
    public async Task<Result<CreateDiplomaCommandResponse>> Handle(CreateDiplomaCommand request, CancellationToken cancellationToken)
    {
        var diploma = new Domain.Entities.Diploma
        {
            Title = request.Title,
            Description = request.Description
        };

        _diplomaRepository.Add(diploma);

        var affectedRows = await _diplomaRepository.SaveChangesAsync(cancellationToken);

        if (affectedRows == 0)
            return Result.Failure<CreateDiplomaCommandResponse>(ResourceErrors.DbError);

        var result = new CreateDiplomaCommandResponse(diploma.Id, diploma.Title, diploma.Description, diploma.Status, diploma.CreatedAt);

        return Result.Success(result);
    }
}
