using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Application;

public static class QuizErrors
{
    public static readonly Error NotFound = new(
       "Quiz.NotFound",
       "Quiz was not found.",
       StatusCodes.Status404NotFound);
}
