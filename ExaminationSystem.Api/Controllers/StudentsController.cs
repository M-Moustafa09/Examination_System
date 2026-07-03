using ExaminationSystem.Api.Extensions;
using ExaminationSystem.Application.Features.Users.Query.GetStudentDashboard;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("dashboard")]
    [Authorize]
    public async Task<IActionResult> StudentDashboard(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetStudentDashboardQuery(User.GetUserId()!), cancellationToken);

        return response.IsSuccess
            ? Ok(response.Value)
            : response.ToProblem();
    }
}
