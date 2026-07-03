using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptWithDetailsV2;

public record GetAttemptWithDetailsV2Query(string StudentId) : IRequest<Result<List<GetAttemptWithDetailsV2Response>>>;