using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Domain.Errors;

public static class QuestionErrors
{
    public static readonly Error NotFound = new(
        "Question.NotFound",
        "Question was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error QuizNotFound = new(
        "Quiz.NotFound",
        "Quiz was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error QuizPublishedConflict = new(
        "Question.QuizPublishedConflict",
        "Cannot modify questions while the quiz is published.",
        StatusCodes.Status409Conflict);

    public static readonly Error OptionNotFound = new(
        "Option.NotFound",
        "Option was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error AnswerForbidden = new(
        "Answer.Forbidden",
        "You are not allowed to answer this question.",
        StatusCodes.Status403Forbidden
    );
}
