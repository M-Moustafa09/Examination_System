using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Update_Diploma;

public class UpdateDiplomaCommandValidator : AbstractValidator<UpdateDiplomaCommand>
{
    public UpdateDiplomaCommandValidator()
    {
        RuleFor(d => d.Id)
            .NotNull()
            .WithMessage("Id cannot be null");
        
        RuleFor(d => d.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .Length(3, 200)
            .WithMessage("The title must be between 3 and 200 characters.")
            .WithErrorCode(StatusCodes.Status422UnprocessableEntity.ToString())
            .When(d => !string.IsNullOrEmpty(d.Title));

        RuleFor(d => d.Description)
            .MaximumLength(1000)
            .WithMessage("The description cannot exceed 1000 characters.")
            .WithErrorCode(StatusCodes.Status422UnprocessableEntity.ToString())
            .When(d => !string.IsNullOrEmpty(d.Description));
    }
}
