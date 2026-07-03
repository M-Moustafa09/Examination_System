using FluentValidation;

namespace ExaminationSystem.Application.Features.Diplomas.Query;

public class GetAllDiplomaQueryValidator : AbstractValidator<GetAllDiplomaQuery>
{
    public GetAllDiplomaQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("PageSize must be between 1 and 100.");
    }
}