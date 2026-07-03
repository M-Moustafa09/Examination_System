using MediatR;

namespace ExaminationSystem.Application.Features.Attempts.Orchestrators.SubmitQuiz;



public record SubmitQuizCommand(Guid AttemptId, string UserId) : IRequest<SubmitQuizResult>;
