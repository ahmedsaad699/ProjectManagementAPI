using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Tasks;

public class BulkUpdateTaskStatusCommand : IRequest<ApiResponse<bool>>
{
    public List<int> TaskIds { get; set; } = new();
    public int Status { get; set; }
}