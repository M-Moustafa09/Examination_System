using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Create_Diploma;

public record CreateDiplomaCommandResponse(Guid Id, string Title, string? Description, DiplomaStatus Status, DateTime CreatedAt);
