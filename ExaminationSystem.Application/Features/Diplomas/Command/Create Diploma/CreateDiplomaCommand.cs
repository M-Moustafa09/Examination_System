using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Create_Diploma;

public record CreateDiplomaCommand(string Title, string? Description) : IRequest<Result<CreateDiplomaCommandResponse>>;
