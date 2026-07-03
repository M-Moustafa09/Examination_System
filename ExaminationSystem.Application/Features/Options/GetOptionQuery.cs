using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Options;

public record GetOptionQuery(
    Guid OptionId
) : IRequest<Result<OptionResponse>>;