using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ProjectManagement.Application.Commands.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;
using ProjectManagement.Shared.Interfaces;  

namespace ProjectManagement.Application.Handlers.Tasks;

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, ApiResponse<TaskDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;
    private readonly INotificationService _notificationService; 

    public UpdateTaskHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMemoryCache cache,
        INotificationService notificationService) // التعديل هنا
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
        _notificationService = notificationService; 
    }

    public async Task<ApiResponse<TaskDto>> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
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

            _mapper.Map(request, task);
            task.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<ProjectTask>().Update(task);
            await _unitOfWork.SaveChangesAsync();

             await _notificationService.NotifyTaskUpdated(task.Id, task.Title);

            // Cache: إزالة البيانات القديمة من الكاش
            var cacheKey = $"task-{task.Id}";
            _cache.Remove(cacheKey);
            _cache.Remove("tasks-list");

            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(task.ProjectId);
            if (project != null)
            {
                _cache.Remove($"project-{project.Id}");
                _cache.Remove("projects-list");
            }

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
                Message = "Task updated successfully",
                Data = taskDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<TaskDto>
            {
                Success = false,
                Message = "An error occurred while updating the task",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}