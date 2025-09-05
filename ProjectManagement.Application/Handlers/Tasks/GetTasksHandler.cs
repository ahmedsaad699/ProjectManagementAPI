using AutoMapper;
using MediatR;
using ProjectManagement.Application.Queries.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Tasks;

public class GetTasksHandler : IRequestHandler<GetTasksQuery, ApiResponse<PagedResultDto<TaskDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTasksHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResultDto<TaskDto>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allTasks = await _unitOfWork.Repository<ProjectTask>().GetAllAsync();

            // Apply filters
            var filteredTasks = allTasks.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                filteredTasks = filteredTasks.Where(t => t.Title.Contains(request.Search) || t.Description.Contains(request.Search));
            }

            if (request.ProjectId.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.ProjectId == request.ProjectId.Value);
            }

            if (request.AssignedToId.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.AssignedToId == request.AssignedToId.Value);
            }

            if (request.Status.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => (int)t.Status == request.Status.Value);
            }

            if (request.Priority.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => (int)t.Priority == request.Priority.Value);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "title":
                        filteredTasks = request.SortDescending ?
                            filteredTasks.OrderByDescending(t => t.Title) :
                            filteredTasks.OrderBy(t => t.Title);
                        break;
                    case "duedate":
                        filteredTasks = request.SortDescending ?
                            filteredTasks.OrderByDescending(t => t.DueDate) :
                            filteredTasks.OrderBy(t => t.DueDate);
                        break;
                    case "priority":
                        filteredTasks = request.SortDescending ?
                            filteredTasks.OrderByDescending(t => t.Priority) :
                            filteredTasks.OrderBy(t => t.Priority);
                        break;
                    default:
                        filteredTasks = filteredTasks.OrderByDescending(t => t.CreatedAt);
                        break;
                }
            }
            else
            {
                filteredTasks = filteredTasks.OrderByDescending(t => t.CreatedAt);
            }

            var totalCount = filteredTasks.Count();

            var tasks = filteredTasks
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var taskDtos = new List<TaskDto>();

            // Map each task with related info
            foreach (var task in tasks)
            {
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
                taskDtos.Add(taskDto);
            }

            return new ApiResponse<PagedResultDto<TaskDto>>
            {
                Success = true,
                Message = "Tasks retrieved successfully",
                Data = new PagedResultDto<TaskDto>
                {
                    Items = taskDtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResultDto<TaskDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving tasks",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}