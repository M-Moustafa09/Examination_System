using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Users.Commands.Register;

public record UserRegisterCommand(string Email, string Password, string FullName) : IRequest<Result<string>>;