using ExaminationSystem.Domain.Enums;

namespace ExaminationSystem.Application.Features.Diplomas.Query;

public record DiplomaResponse(
    string Title,
    string Description,
    DiplomaStatus Status
);