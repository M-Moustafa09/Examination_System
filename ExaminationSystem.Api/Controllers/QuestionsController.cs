using ExaminationSystem.Application.Features.Questions.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[Route("api/admin/questions")]
[ApiController]
public class QuestionsController(ISender _sender) : ControllerBase
{
    [HttpPut("{questionId:int}")]
    public async Task<IActionResult> UpdateQuestion([FromRoute] int questionId, [FromBody] UpdateQuestionRequest request)
    {
        var command = new UpdateQuestionCommand(
            questionId,
            request.Text,
            request.Explanation,
            request.OrderIndex,
            request.Options);

        var result = await _sender.Send(command);

        return result.IsSuccess
            ? Ok()
            : result.ToProblem();
    }

    [HttpDelete("{questionId:int}")]
    public async Task<IActionResult> DeleteQuestion([FromRoute] int questionId)
    {
        var result = await _sender.Send(new DeleteQuestionCommand(questionId));

        return result.IsSuccess
            ? NoContent()
            : result.ToProblem();
    }
}

public record UpdateQuestionRequest(
    string Text,
    string? Explanation,
    int OrderIndex,
    List<QuestionOptionRequest> Options
);
