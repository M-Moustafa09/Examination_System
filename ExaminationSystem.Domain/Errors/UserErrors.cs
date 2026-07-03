using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Domain.Errors;

public static class UserErrors
{
    public static readonly Error DuplicateEmail = new(
        "User.DuplicateEmail",
        "The provided email is already in use.",
        StatusCodes.Status409Conflict);

    public static readonly Error NotFound = new(
        "User.NotFound",
        "The user with the specified ID was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The email or password provided is incorrect.",
        StatusCodes.Status401Unauthorized);

    public static readonly Error UpdateFailed = new(
        "User.UpdateFailed",
        "An error occurred while updating the user information.",
        StatusCodes.Status500InternalServerError);

    public static readonly Error EmailNotConfirmed = new(
        "User.EmailNotConfirmed",
        "The email address has not been confirmed.",
        StatusCodes.Status403Forbidden);

    public static readonly Error LockedOut = new(
        "User.LockedOut",
        "The user account is locked out.",
        StatusCodes.Status403Forbidden);
}