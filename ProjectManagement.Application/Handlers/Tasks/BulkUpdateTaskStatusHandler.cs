using MediatR;
using ProjectManagement.Application.Commands.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Handlers.Tasks;

public class BulkUpdateTaskStatusHandler : IRequestHandler<BulkUpdateTaskStatusCommand, ApiResponse<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public BulkUpdateTaskStatusHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<bool>> Handle(BulkUpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _unitOfWork.Repository<ProjectTask>()
                .FindAsync(t => request.TaskIds.Contains(t.Id));

            if (!tasks.Any())
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "No tasks found"
                };
            }

            foreach (var task in tasks)
            {
                task.Status = (Domain.Enums.TaskStatus)request.Status;
                task.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Repository<ProjectTask>().Update(task);
            }

            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Tasks status updated successfully",
                Data = true
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "An error occurred while updating tasks status",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}