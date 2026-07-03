using ExaminationSystem.Application.Features.Admins;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AdminsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("stats")]
    public async Task<IActionResult> Stats(CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new GetAdminStatsQuery(), cancellationToken);
        return Ok(response.Value);
    }
}
