//using ExaminationSystem.Domain.Abstractions;
//using ExaminationSystem.Domain.Entities;
//using ExaminationSystem.Domain.Enums;
//using ExaminationSystem.Domain.Errors;
//using ExaminationSystem.Domain.Repositories;
//using MediatR;
//using Microsoft.EntityFrameworkCore;

//namespace ExaminationSystem.Application.Features.Atempts.Queries.StartQuizCheck;

//public class StartQuizCheckQueryHandler(IGeneralRepository<Attempt> generalRepository) : IRequestHandler<StartQuizCheckQuery, Result<int>>
//{
//    private readonly IGeneralRepository<Attempt> _generalRepository = generalRepository;

//    public async Task<Result<int>> Handle(StartQuizCheckQuery request, CancellationToken cancellationToken)
//    {
//        var inProgressAttemt = await _generalRepository.GetTable().FirstOrDefaultAsync(a => a.QuizId == request.QuizId && a.Status == AttemptStatus.InProgress, cancellationToken);


//        if (inProgressAttemt is not null)
//            return Result.Failure<int>(AttemptErrors.AlreadyInProgres);


//    }
//}
