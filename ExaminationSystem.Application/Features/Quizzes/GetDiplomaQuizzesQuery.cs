using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Quizzes;

public record GetDiplomaQuizzesQuery(Guid DiplomaId, string StudentId)
    : IRequest<Result<List<DiplomaQuizzesQueryResponse>>>;