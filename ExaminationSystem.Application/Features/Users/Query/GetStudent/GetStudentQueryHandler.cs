using ExaminationSystem.Domain.Abstractions;
using ExaminationSystem.Domain.Entities;
using ExaminationSystem.Domain.Errors;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Users.Query.GetStudent;

public class GetStudentQueryHandler(IGeneralRepository<ApplicationUser> generalRepository) : IRequestHandler<GetStudentQuery, Result<GetStudentQueryResponse>>
{
    private readonly IGeneralRepository<ApplicationUser> _generalRepository = generalRepository;

    public async Task<Result<GetStudentQueryResponse>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await _generalRepository.GetTable()
        .AsNoTracking()
        .Where(s => s.Id == request.StudentId)
        .Select(p => new GetStudentQueryResponse(p.Id, p.FullName, p.Email!))
        .FirstOrDefaultAsync(cancellationToken);

        return student is null
            ? Result.Failure<GetStudentQueryResponse>(UserErrors.NotFound)
            : Result.Success(student);
    }
}
