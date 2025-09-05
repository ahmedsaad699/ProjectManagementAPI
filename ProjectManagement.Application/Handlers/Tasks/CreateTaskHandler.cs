using AutoMapper;
using MediatR;
using ProjectManagement.Application.Commands.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Tasks;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, ApiResponse<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTaskHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var task = _mapper.Map<ProjectTask>(request);

            await _unitOfWork.Repository<ProjectTask>().AddAsync(task);
            await _unitOfWork.SaveChangesAsync();

            // Reload with related data
            var createdTask = await _unitOfWork.Repository<ProjectTask>()
                .SingleOrDefaultAsync(t => t.Id == task.Id);

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(createdTask.ProjectId);
            var assignedTo = createdTask.AssignedToId.HasValue ?
                await _unitOfWork.Repository<User>().GetByIdAsync(createdTask.AssignedToId.Value) : null;

            var taskDto = _mapper.Map<TaskDto>(createdTask);
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
                Message = "Task created successfully",
                Data = taskDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TaskDto>
            {
                Success = false,
                Message = "An error occurred while creating the task",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}