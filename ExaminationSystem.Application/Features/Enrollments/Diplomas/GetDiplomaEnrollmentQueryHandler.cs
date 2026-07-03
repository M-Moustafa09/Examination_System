using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Enrollments.Diplomas;

public class GetDiplomaEnrollmentQueryHandler(IGeneralRepository<Enrollment> _generalRepository)
    : IRequestHandler<GetDiplomaEnrollmentQuery, Result<bool>>
{
    public async Task<Result<bool>> Handle(GetDiplomaEnrollmentQuery request, CancellationToken cancellationToken)
    {
        var isEnrollment = await _generalRepository.GetTable()
            .AnyAsync(e => e.DiplomaId == request.DiaplomaId && e.StudentId == request.StudentId, cancellationToken);
        return Result.Success(isEnrollment);
    }
}