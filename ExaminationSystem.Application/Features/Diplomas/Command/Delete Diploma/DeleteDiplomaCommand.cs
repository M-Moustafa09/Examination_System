using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Delete_Diploma;

public record DeleteDiplomaCommand(Guid Id) : IRequest<Result>;
