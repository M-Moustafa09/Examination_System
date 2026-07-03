using ExaminationSystem.Application.Features.Admin.Queries.GetAdminStats;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[ApiController]
[Route("api/admin")]
//[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }
    //[Authorize(Roles = "Admin")]
    [HttpGet("stats")] 
    public async Task<IActionResult> GetStats(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAdminStatsQuery(), cancellationToken);
        return Ok(result);
    }
}
