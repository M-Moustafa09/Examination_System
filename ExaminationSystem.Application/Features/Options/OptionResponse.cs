namespace ExaminationSystem.Application.Features.Options;

public record OptionResponse(
    Guid Id,
    Guid QuestionId,
    string Text,
    bool IsCorrect
    );