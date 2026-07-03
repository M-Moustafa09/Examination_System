using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Create_Diploma;

public class CreateDiplomaCommandValidator : AbstractValidator<CreateDiplomaCommand>
{
    public CreateDiplomaCommandValidator()
    {
        RuleFor(d => d.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Length(3, 200)
            .WithMessage("The title must be between 3 and 200 characters.")
            .WithErrorCode(StatusCodes.Status422UnprocessableEntity.ToString());

        RuleFor(d => d.Description)
            .MaximumLength(1000)
            .WithMessage("The description cannot exceed 1000 characters.")
            .WithErrorCode(StatusCodes.Status422UnprocessableEntity.ToString())
            .When(d => !string.IsNullOrEmpty(d.Description));
    }
}
