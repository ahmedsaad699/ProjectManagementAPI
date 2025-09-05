using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProjectManagement.Application.Commands.Tasks;
using ProjectManagement.Application.Queries.Tasks;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<PagedResultDto<TaskDto>>>> GetTasks([FromQuery] GetTasksQuery query)
    {
        var result = await _mediator.Send(query);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<TaskDto>>> CreateTask([FromBody] CreateTaskDto dto)
    {
        var command = new CreateTaskCommand
        {
            ProjectId = dto.ProjectId,
            Title = dto.Title,
            Description = dto.Description,
            AssignedToId = dto.AssignedToId,
            Priority = dto.Priority,
            DueDate = dto.DueDate
        };

        var result = await _mediator.Send(command);

        if (result.Success)
            return CreatedAtAction(nameof(GetTasks), result);

        return BadRequest(result);
    }

    [HttpPut("bulk-status")]
    public async Task<ActionResult<ApiResponse<bool>>> BulkUpdateStatus([FromBody] BulkUpdateTaskStatusDto dto)
    {
        var command = new BulkUpdateTaskStatusCommand
        {
            TaskIds = dto.TaskIds,
            Status = dto.Status
        };

        var result = await _mediator.Send(command);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}