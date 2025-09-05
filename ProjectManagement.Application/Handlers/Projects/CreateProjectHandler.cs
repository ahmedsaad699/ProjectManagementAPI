using AutoMapper;
using MediatR;
using ProjectManagement.Application.Commands.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Projects;

public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ApiResponse<ProjectDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProjectHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = _mapper.Map<Project>(request);

            await _unitOfWork.Repository<Project>().AddAsync(project);
            await _unitOfWork.SaveChangesAsync();

            // Reload with manager data
            var createdProject = await _unitOfWork.Repository<Project>()
                .SingleOrDefaultAsync(p => p.Id == project.Id);

            var projectDto = _mapper.Map<ProjectDto>(createdProject);

            return new ApiResponse<ProjectDto>
            {
                Success = true,
                Message = "Project created successfully",
                Data = projectDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<ProjectDto>
            {
                Success = false,
                Message = "An error occurred while creating the project",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}