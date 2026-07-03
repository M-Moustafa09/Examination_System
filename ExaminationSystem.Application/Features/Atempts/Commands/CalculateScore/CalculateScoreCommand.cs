using MediatR;

namespace ExaminationSystem.Application.Features.Attempts.Commands.CalculateScore;

public record CalculateScoreCommand(Guid AttemptId, decimal PassScore) : IRequest<CalculateScoreResponse>;