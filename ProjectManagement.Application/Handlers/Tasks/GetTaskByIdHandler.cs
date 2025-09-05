using AutoMapper;
using MediatR;
using ProjectManagement.Application.Queries.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Tasks;

public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, ApiResponse<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTaskByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var task = await _unitOfWork.Repository<ProjectTask>().GetByIdAsync(request.Id);

            if (task == null)
            {
                return new ApiResponse<TaskDto>
                {
                    Success = false,
                    Message = "Task not found"
                };
            }

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(task.ProjectId);
            var assignedTo = task.AssignedToId.HasValue ?
                await _unitOfWork.Repository<User>().GetByIdAsync(task.AssignedToId.Value) : null;

            var taskDto = _mapper.Map<TaskDto>(task);
            if (project != null)
            {
                taskDto.ProjectName = project.Name;
            }
            if (assignedTo != null)
            {
                taskDto.AssignedToName = $"{assignedTo.FirstName} {assignedTo.LastName}";
            }

            return new ApiResponse<TaskDto>
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = taskDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TaskDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the task",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}