using MediatR;
using ExaminationSystem.Domain.Abstractions;

namespace ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptById;

public class GetAttemptByIdQuery : IRequest<Result<GetAttemptByIdResponse>>
{
    public Guid Id { get; set; }

    public GetAttemptByIdQuery(Guid id)
    {
        Id = id;
    }
}
