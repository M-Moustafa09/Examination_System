using ExaminationSystem.Domain.Abstractions;
using Microsoft.AspNetCore.Http;

namespace ExaminationSystem.Domain.Errors;

public static class AttemptErrors
{
    public static readonly Error NoInProgresAttempt = new(
      "Attempt.NoInProgresAttempt",
      "No in-progress attempt exists.",
      StatusCodes.Status409Conflict);

    public static readonly Error InProgresAttempt = new(
      "Attempt.InProgresAttempt",
      "in-progress attempt exists.",
      StatusCodes.Status409Conflict);



    public static readonly Error MaxAttempts = new(
      "Attempt.MaxAttempts",
      "Your Attempts reach Max Attempts",
      StatusCodes.Status409Conflict);

    public static readonly Error NotFound = new(
        "RES_ATTEMPT_NOT_FOUND",
        "Attempt was not found.",
        StatusCodes.Status404NotFound);

    public static readonly Error AccessDenied = new(
        "QUIZ_ATTEMPT_NOT_OWNER",
        "You do not have access to this attempt.",
        StatusCodes.Status403Forbidden);

    public static readonly Error ResultsNotReady = new(
        "QUIZ_RESULTS_NOT_READY",
        "Results are only available after the attempt is submitted or timed out.",
        StatusCodes.Status403Forbidden);

    public static readonly Error TimedOut = new(
        "QUIZ_ATTEMPT_TIMED_OUT",
        "This attempt has timed out and can no longer be answered.",
        StatusCodes.Status410Gone);

    public static readonly Error DeadlinePassed = new(
        "QUIZ_ATTEMPT_DEADLINE_PASSED",
        "The deadline for this attempt has passed.",
        StatusCodes.Status410Gone);
}
