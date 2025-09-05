using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProjectManagement.Application.Commands.Auth;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginCommand
        {
            Username = request.Username,
            Password = request.Password
        };

        var result = await _mediator.Send(command);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<string>>> Register([FromBody] RegisterRequestDto request)
    {
        var command = new RegisterCommand
        {
            Username = request.Username,
            Email = request.Email,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role
        };

        var result = await _mediator.Send(command);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}