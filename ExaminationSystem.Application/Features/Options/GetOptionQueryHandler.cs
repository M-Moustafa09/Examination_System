using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;

namespace ExaminationSystem.Application.Features.Options;

public class GetOptionQueryHandler(IGeneralRepository<Option> _optionRepository) : IRequestHandler<GetOptionQuery, Result<OptionResponse>>
{
    public async Task<Result<OptionResponse>> Handle(GetOptionQuery request, CancellationToken cancellationToken)
    {
        var option = _optionRepository.GetById(request.OptionId);

        if (option is null || option.IsDeleted)
            return Result.Failure<OptionResponse>(OptionErrors.NotFound);


        return Result.Success(new OptionResponse(
            Id: option.Id,
            QuestionId: option.QuestionId,
            Text: option.Text,
            IsCorrect: option.IsCorrect
        ));
    }
}