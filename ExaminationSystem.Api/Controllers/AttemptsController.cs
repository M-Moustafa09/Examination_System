using ExaminationSystem.Api;
using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Application.Features.Atempts.Orchestrators.AnswerQuestion;
using ExaminationSystem.Application.Features.Atempts.Queries.GetAttemptResults;
using ExaminationSystem.Application.Features.Atempts.Queries.GetStudentAttempts;
using ExaminationSystem.Application.Features.Attempts.Orchestrators.SubmitQuiz;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/attempts")]
//[Authorize(Roles = "student")]   // Only students can submit
public class AttemptsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AttemptsController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [Authorize]
    [HttpGet("{attemptId:guid}/results")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAttemptResults(Guid attemptId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var result = await _mediator.Send(new GetAttemptResultsQuery(attemptId, userId), cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    /// <summary>
    /// Get paginated list of student's quiz attempts (US 3.6)
    /// GET /api/student/attempts
    /// </summary>
    [Authorize]
    [HttpGet("getattemptStudent")]
    public async Task<IActionResult> GetStudentAttempts(
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 10,
       CancellationToken cancellationToken = default)
    {
        //try
        //{
        // Get student ID from authenticated user
        var userIdString = User.GetUserId();
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized(new { message = "Unable to determine student ID." });
        }
        //int studentId = 3;

        // Validate pagination parameters
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Max 100 per page

        var query = new GetStudentAttemptsQuery(userIdString, page, pageSize);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Error.Message });
        }

        // Return paginated response
        return Ok(new
        {
            data = result.Value.Data,
            page = result.Value.Page,
            pageSize = result.Value.PageSize,
            totalCount = result.Value.TotalCount,
            totalPages = result.Value.TotalPages
        });
        //  }
        //catch (Exception ex)
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError,
        //        new { message = "An error occurred while retrieving attempts.", details = ex.Message });
        // }
    }

    /// <summary>
    /// Get detailed information for a specific attempt (US 3.6)
    /// GET /api/student/attempts/{attemptId}
    /// </summary>
    [Authorize]
    [HttpGet("getattemptDetails")]
    public async Task<IActionResult> GetAttemptDetails(Guid attemptId, CancellationToken cancellationToken)
    {
        //try
        //{
        // Get student ID from authenticated user
        var userIdString = User.GetUserId();
        if (string.IsNullOrEmpty(userIdString))
        {
            return Unauthorized(new { message = "Unable to determine student ID." });
        }

        // Query with attempt ID filter (only returns data if student owns this attempt)
        var query = new GetStudentAttemptsQuery(userIdString, Page: 1, PageSize: 1, AttemptIdFilter: attemptId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { message = result.Error.Message });
        }

        if (result.Value.Data.Count() == 0)
        {
            return NotFound(new { message = $"Attempt with ID {attemptId} not found." });
        }

        // Return the single attempt
        return Ok(result.Value.Data.First());
        //}
        //catch (Exception ex)
        //{
        //    return StatusCode(StatusCodes.Status500InternalServerError,
        //        new { message = "An error occurred while retrieving the attempt.", details = ex.Message });
        //}
    }
    [Authorize]
    [HttpPost("/submitquiz")]
    public async Task<IActionResult> SubmitQuiz(Guid attemptId, CancellationToken cancellationToken)
    {
        var userIdString = User.GetUserId();
        if (string.IsNullOrEmpty(userIdString))
            return Unauthorized();
        
        var command = new SubmitQuizCommand(attemptId, userIdString); 

        //try
        //{
            var result = await _mediator.Send(command, cancellationToken);

            
            return Ok(new
            {
                attempt_id = result.AttemptId,
                score = result.Score,
                passed = result.Passed,
                status = result.Status   
            });
        //}
        //catch (KeyNotFoundException)
        //{
        //    return NotFound(new { message = $"Attempt with ID {attemptId} not found." });
        //}
        //catch (UnauthorizedAccessException)
        //{
        //    return Forbid(); // 403 Forbidden - user does not own this attempt
        //}
        //catch (InvalidOperationException ex) when (ex.Message.Contains("already submitted"))
        //{
        //    return Conflict(new { message = "Attempt is already submitted or timed out." });
        //}
        //catch (InvalidOperationException ex) // any other invalid state
        //{
        //    return BadRequest(new { message = ex.Message });
        //}
    }



    [ProducesResponseType(typeof(AnswerQuestionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status410Gone)]
    [HttpPost("{attemptId:guid}/answer")]
    public async Task<IActionResult> AnswerQuestion([FromRoute] Guid attemptId, AnswerQuestionRequest request, CancellationToken cancellationToken)
    {

        var result = await _mediator.Send(new AnswerQuestionOrchestrator(attemptId, request.QuestionId, request.SelectedOptionId), cancellationToken);
        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}

