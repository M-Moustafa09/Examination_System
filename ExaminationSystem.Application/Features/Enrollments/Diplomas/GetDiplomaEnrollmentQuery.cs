using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Enrollments.Diplomas;

public record GetDiplomaEnrollmentQuery(Guid DiaplomaId, string StudentId) : IRequest<Result<bool>>;