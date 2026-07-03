using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Users.Query.GetStudent;

public record GetStudentQuery(string StudentId) : IRequest<Result<GetStudentQueryResponse>>;