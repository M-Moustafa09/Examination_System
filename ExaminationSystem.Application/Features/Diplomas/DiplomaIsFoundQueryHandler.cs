using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas;

public class DiplomaIsFoundQueryHandler(IGeneralRepository<Diploma> generalRepository)
    : IRequestHandler<DiplomaIsFoundQuery, Result<bool>>
{
    private readonly IGeneralRepository<Diploma> _generalRepository = generalRepository;

    public async Task<Result<bool>> Handle(DiplomaIsFoundQuery request, CancellationToken cancellationToken)
    {
        var isFound = await _generalRepository.GetTable()
            .AnyAsync(d => d.Id == request.DiplomaId && d.Status == DiplomaStatus.Published, cancellationToken);
        return Result.Success(isFound);
    }
}