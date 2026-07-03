using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas;

public record DiplomaIsFoundQuery(Guid DiplomaId) : IRequest<Result<bool>>;