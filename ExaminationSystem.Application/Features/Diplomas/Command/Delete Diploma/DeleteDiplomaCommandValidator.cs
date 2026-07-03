using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Application.Features.Diplomas.Command.Delete_Diploma;

public class DeleteDiplomaCommandValidator : AbstractValidator<DeleteDiplomaCommand>
{
    public DeleteDiplomaCommandValidator()
    {
        RuleFor(d => d.Id)
            .NotNull()
            .WithMessage("Id cannot be null");
    }
}
