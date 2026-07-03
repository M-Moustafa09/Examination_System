using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Domain.Errors;

public static class OptionErrors
{
    public static readonly Error NotFound = new(
        "Option.NotFound",
        "Option was not found.",
        StatusCodes.Status404NotFound);
}
