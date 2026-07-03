using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Update_Diploma;

public record UpdateDiplomaCommand(Guid Id, string? Title, string? Description) : IRequest<Result<UpdateDiplomaCommandResponse>>;
