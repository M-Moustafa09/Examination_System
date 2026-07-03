using ExaminationSystem.Application.Features.Users.Commands.Login;
using ExaminationSystem.Application.Features.Users.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExaminationSystem.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class AuthController(ISender _sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterCommand request)
    {
        var result = await _sender.Send(request);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginCommand command, CancellationToken ct)
    {
        var result = await _sender.Send(command, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : result.ToProblem();
    }
}