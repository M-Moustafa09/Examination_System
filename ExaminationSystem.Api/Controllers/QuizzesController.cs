using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Application.Features.Atempts.Orchestrators.Start_Quiz;
using ExaminationSystem.Application.Features.Questions.Command;
using ExaminationSystem.Application.Features.Quizzes.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizzesController(ISender _sender) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateQuiz(CreateQuizCommand command)
    {
        var result = await _sender.Send(command);


        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }

    [HttpPost("/api/admin/quizzes/{quizId:int}/questions")]
    public async Task<IActionResult> AddQuestion([FromRoute] int quizId, [FromBody] AddQuestionRequest request)
    {
        var command = new AddQuestionCommand(
            quizId,
            request.Text,
            request.Explanation,
            request.OrderIndex,
            request.Options);

        var result = await _sender.Send(command);

        return result.IsSuccess
            ? StatusCode(StatusCodes.Status201Created, new { questionId = result.Value })
            : result.ToProblem();
    }
    [Authorize]
    [HttpPost("{quizId}/start")]
    public async Task<IActionResult> StartQuiz([FromRoute] Guid quizId, CancellationToken cancellationToken)
    {
        var response = await _sender.Send(new StartQuizOrchestratorCommand(quizId, User.GetUserId()!), cancellationToken);

        return response.IsSuccess ?
            Ok(response.Value)
            : response.ToProblem();
    }
}

public record AddQuestionRequest(
    string Text,
    string? Explanation,
    int OrderIndex,
    List<QuestionOptionRequest> Options
);