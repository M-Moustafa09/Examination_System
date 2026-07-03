using FluentValidation;

namespace ExaminationSystem.Application.Features.Users.Commands.Login;

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");
    }
}