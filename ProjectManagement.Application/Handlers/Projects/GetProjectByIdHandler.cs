using AutoMapper;
using MediatR;
using ProjectManagement.Application.Queries.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Projects;

public class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ApiResponse<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProjectByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProjectDto>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.Id);

            if (project == null)
            {
                return new ApiResponse<ProjectDto>
                {
                    Success = false,
                    Message = "Project not found"
                };
            }

            var manager = await _unitOfWork.Repository<User>().GetByIdAsync(project.ManagerId);
            var tasks = await _unitOfWork.Repository<ProjectTask>()
                .FindAsync(t => t.ProjectId == project.Id);

            var projectDto = _mapper.Map<ProjectDto>(project);
            if (manager != null)
            {
                projectDto.ManagerName = $"{manager.FirstName} {manager.LastName}";
            }
            projectDto.TasksCount = tasks.Count();

            return new ApiResponse<ProjectDto>
            {
                Success = true,
                Message = "Project retrieved successfully",
                Data = projectDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProjectDto>
            {
                Success = false,
                Message = "An error occurred while retrieving the project",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}