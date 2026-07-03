namespace ExaminationSystem.Application.Features.Questions.Query.ShuffleQuestions;

public record ShuffleQuestionResponse(Guid QuestionId, string Text, int DisplayOrder, List<OptionDto> Options);

public record OptionDto(Guid OptionId, string Text);