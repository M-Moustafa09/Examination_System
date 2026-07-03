using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Query.GetEnrolledDiplomas;

public record GetEnrolledDiplomasQuery(string StudentId) : IRequest<Result<GetEnrolledDiplomasWithRecentAttemptsResponse>>;