 
using MediatR;
using ProjectManagement.Application.Commands.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Projects;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteProjectHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _unitOfWork.Repository<Project>().GetByIdAsync(request.Id);

            if (project == null)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Project not found"
                };
            }

            // Soft delete
            project.IsDeleted = true;
            project.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Project deleted successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "An error occurred while deleting the project",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}