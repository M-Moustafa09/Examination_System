using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Update_Diploma;

public record UpdateDiplomaRequest(string? Title, string? Description);
public record UpdateDiplomaCommandResponse(Guid Id, string Title, string? Description, DiplomaStatus Status, DateTime CreatedAt, DateTime UpdatedAt, bool IsDeleted, DateTime DeletedAt);
