using ExaminationSystem.Application.Common;
using ExaminationSystem.Domain.Enums;
using ExaminationSystem.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Application.Features.Diplomas.Query;

public class GetAllDiplomaQueryHandler(IDiplomaRepository _repo)
    : IRequestHandler<GetAllDiplomaQuery, PagedResponse<DiplomaResponse>>
{
    public async Task<PagedResponse<DiplomaResponse>> Handle(GetAllDiplomaQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetTable();

        // Filter

        query = query.Where(d => d.Status == DiplomaStatus.Published);

        var totalCount = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(v => new DiplomaResponse(
                v.Title,
                v.Description,
                v.Status
            ))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        var paged = new PagedResponse<DiplomaResponse>
        {
            Data = data,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages
        };

        return paged;
    }
}