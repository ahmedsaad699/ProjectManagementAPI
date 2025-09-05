using AutoMapper;
using MediatR;
using ProjectManagement.Application.Commands.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Projects;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, ApiResponse<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProjectHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
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

            _mapper.Map(request, project);
            project.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.SaveChangesAsync();

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
                Message = "Project updated successfully",
                Data = projectDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProjectDto>
            {
                Success = false,
                Message = "An error occurred while updating the project",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}