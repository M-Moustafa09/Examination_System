using ExaminationSystem.Application.Features.Attempts.Queries.GetAttemptById;
using ExaminationSystem.Application.Features.Attempts.Queries.GetAttempts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers
{
    [ApiController]
    [Route("api/admin/attempts")]
    public class AdminAttemptController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminAttemptController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/admin/attempts
        [HttpGet]
        public async Task<IActionResult> GetAttempts([FromQuery] GetAttemptsQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        // GET api/admin/attempts/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAttemptById(Guid id)
        {
            var result = await _mediator.Send(
                new GetAttemptByIdQuery(id));

            return Ok(result);
        }
    }
}





