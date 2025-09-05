using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProjectManagement.Application.Commands.Projects;
using ProjectManagement.Application.Queries.Projects;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResultDto<ProjectDto>>>> GetProjects([FromQuery] GetProjectsListQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(int id)
    {
        var query = new GetProjectByIdQuery { Id = id };
        var result = await _mediator.Send(query);

        if (result.Success)
            return Ok(result);

        return NotFound(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,ProjectManager")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> CreateProject([FromBody] CreateProjectDto dto)
    {
        var command = new CreateProjectCommand
        {
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Budget = dto.Budget,
            ManagerId = dto.ManagerId
        };

        var result = await _mediator.Send(command);

        if (result.Success)
            return CreatedAtAction(nameof(GetProject), new { id = result.Data!.Id }, result);

        return BadRequest(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,ProjectManager")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> UpdateProject(int id, [FromBody] UpdateProjectDto dto)
    {
        var command = new UpdateProjectCommand
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            Budget = dto.Budget
        };

        var result = await _mediator.Send(command);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,ProjectManager")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteProject(int id)
    {
        var command = new DeleteProjectCommand { Id = id };
        var result = await _mediator.Send(command);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}