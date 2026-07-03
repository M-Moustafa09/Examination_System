using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Application.Common;
using ExaminationSystem.Application.Features.Diplomas.Command.Create_Diploma;
using ExaminationSystem.Application.Features.Diplomas.Command.Delete_Diploma;
using ExaminationSystem.Application.Features.Diplomas.Command.Update_Diploma;
using ExaminationSystem.Application.Features.Diplomas.Query;
using ExaminationSystem.Application.Features.Quizzes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class DiplomasController(ISender _sender) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResponse<DiplomaResponse>))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDiplomaQuery query, CancellationToken ct)
    {
        var response = await _sender.Send(query, ct);
        return Ok(response);
    }

    [HttpGet("{diplomaId}/quizzes")]
    [Authorize]
    public async Task<IActionResult> GetDiplomaQuizzes([FromRoute] Guid diplomaId)
    {
        var result = await _sender.Send(new GetDiplomaQuizzesQuery(diplomaId, User.GetUserId()!));

        return result.IsSuccess
            ? Ok(result)
            : result.ToProblem();
    }

    [HttpPost("~/api/admin/diplomas")]
    [Authorize]
    public async Task<IActionResult> Add([FromBody] CreateDiplomaCommand requst)
    {
        var response = await _sender.Send(requst);
        return response.IsSuccess
            ? Created(string.Empty, response.Value)
            : response.ToProblem();
    }

    [HttpPut("~/api/admin/diplomas/{diplomaId}")]
    [Authorize]
    public async Task<IActionResult> Update([FromRoute] Guid diplomaId, [FromBody] UpdateDiplomaRequest request)
    {
        var response = await _sender.Send(new UpdateDiplomaCommand(diplomaId, request.Title, request.Description));
        return response.IsSuccess
            ? Ok(response)
            : response.ToProblem();
    }

    [HttpDelete("~/api/admin/diplomas/{diplomaId}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid diplomaId)
    {
        var response = await _sender.Send(new DeleteDiplomaCommand(diplomaId));
        return response.IsSuccess
            ? Ok(response)
            : response.ToProblem();
    }
}