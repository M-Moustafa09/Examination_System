using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Domain.Errors;

public static class ResourceErrors
{
    public static readonly Error DbError = new("SRV_DATABASE_ERROR", "Database operation failed",
        StatusCodes.Status500InternalServerError);
}