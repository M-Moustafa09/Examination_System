using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Users.Query.GetStudentDashboard;

public record GetStudentDashboardQuery(string StudentId) : IRequest<Result<GetStudentDashboardResponse>>;