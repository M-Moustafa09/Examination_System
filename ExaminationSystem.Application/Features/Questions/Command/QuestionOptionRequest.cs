namespace ExaminationSystem.Application.Features.Questions.Command;

public record QuestionOptionRequest(
    string Text,
    bool IsCorrect,
    int OrderIndex
);
