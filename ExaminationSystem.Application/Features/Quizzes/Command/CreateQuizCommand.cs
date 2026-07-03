using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Quizzes.Command;

public record CreateQuizCommand(
    string Title,
    Guid DiplomaId,
    string? Instructions,
    int DurationMinutes,
    decimal PassScore = 60,
    int? MaxAttempts = null
) : IRequest<Result<Guid>>;