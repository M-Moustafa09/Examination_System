using ExaminationSystem.Domain.Abstractions;
using MediatR;

namespace ExaminationSystem.Application.Features.Questions.Command;

public record AddQuestionCommand(
    int QuizId,
    string Text,
    string? Explanation,
    int OrderIndex,
    List<QuestionOptionRequest> Options
) : IRequest<Result<int>>;
