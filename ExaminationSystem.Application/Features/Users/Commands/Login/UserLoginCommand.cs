using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Users.Commands.Login;

public record UserLoginCommand(string Email, string Password) : IRequest<Result<UserLoginResponse>>;