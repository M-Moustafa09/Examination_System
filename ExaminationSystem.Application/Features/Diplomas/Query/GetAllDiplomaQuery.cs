using ExaminationSystem.Application.Common;
using MediatR;

namespace ExaminationSystem.Application.Features.Diplomas.Query;

public record GetAllDiplomaQuery(
    int Page = 1,
    int PageSize = 20
) : IRequest<PagedResponse<DiplomaResponse>>;