using AutoMapper;
using MediatR;
using ProjectManagement.Application.Commands.Projects;
using ProjectManagement.Application.Queries.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Projects;

public class GetProjectsListHandler : IRequestHandler<GetProjectsListQuery, ApiResponse<PagedResultDto<ProjectDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProjectsListHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResultDto<ProjectDto>>> Handle(GetProjectsListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var allProjects = await _unitOfWork.Repository<Project>().GetAllAsync();

            // Apply filters
            var filteredProjects = allProjects.AsQueryable();

            if (!string.IsNullOrEmpty(request.Search))
            {
                filteredProjects = filteredProjects.Where(p => p.Name.Contains(request.Search) || p.Description.Contains(request.Search));
            }

            if (request.Status.HasValue)
            {
                filteredProjects = filteredProjects.Where(p => (int)p.Status == request.Status.Value);
            }

            if (request.ManagerId.HasValue)
            {
                filteredProjects = filteredProjects.Where(p => p.ManagerId == request.ManagerId.Value);
            }

            // Apply sorting
            if (!string.IsNullOrEmpty(request.SortBy))
            {
                switch (request.SortBy.ToLower())
                {
                    case "name":
                        filteredProjects = request.SortDescending ?
                            filteredProjects.OrderByDescending(p => p.Name) :
                            filteredProjects.OrderBy(p => p.Name);
                        break;
                    case "startdate":
                        filteredProjects = request.SortDescending ?
                            filteredProjects.OrderByDescending(p => p.StartDate) :
                            filteredProjects.OrderBy(p => p.StartDate);
                        break;
                    case "budget":
                        filteredProjects = request.SortDescending ?
                            filteredProjects.OrderByDescending(p => p.Budget) :
                            filteredProjects.OrderBy(p => p.Budget);
                        break;
                    default:
                        filteredProjects = filteredProjects.OrderByDescending(p => p.CreatedAt);
                        break;
                }
            }
            else
            {
                filteredProjects = filteredProjects.OrderByDescending(p => p.CreatedAt);
            }

            var totalCount = filteredProjects.Count();

            var projects = filteredProjects
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var projectDtos = new List<ProjectDto>();

            // Map each project with manager info
            foreach (var project in projects)
            {
                var manager = await _unitOfWork.Repository<User>().GetByIdAsync(project.ManagerId);
                var tasks = await _unitOfWork.Repository<ProjectTask>()
                    .FindAsync(t => t.ProjectId == project.Id);

                var projectDto = _mapper.Map<ProjectDto>(project);
                if (manager != null)
                {
                    projectDto.ManagerName = $"{manager.FirstName} {manager.LastName}";
                }
                projectDto.TasksCount = tasks.Count();
                projectDtos.Add(projectDto);
            }

            return new ApiResponse<PagedResultDto<ProjectDto>>
            {
                Success = true,
                Message = "Projects retrieved successfully",
                Data = new PagedResultDto<ProjectDto>
                {
                    Items = projectDtos,
                    TotalCount = totalCount,
                    PageNumber = request.PageNumber,
                    PageSize = request.PageSize
                }
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PagedResultDto<ProjectDto>>
            {
                Success = false,
                Message = "An error occurred while retrieving projects",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}