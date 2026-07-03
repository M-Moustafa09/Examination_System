using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Domain.Errors;

public static class DiplomaErrors
{
    public static readonly Error NotFound = new(
        "Diploma.NotFound",
        "The specified diploma does not exist or has been deleted.",
        StatusCodes.Status404NotFound);

    public static readonly Error NotEnrolled = new(
        "Enrollment.NotEnrolled",
        "The student is not enrolled in this diploma.",
        StatusCodes.Status403Forbidden);
}