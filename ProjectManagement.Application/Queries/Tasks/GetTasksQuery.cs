using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Queries.Tasks;

public class GetTasksQuery : IRequest<ApiResponse<PagedResultDto<TaskDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
    public string? Search { get; set; }
    public int? ProjectId { get; set; }
    public int? AssignedToId { get; set; }
    public int? Status { get; set; }
    public int? Priority { get; set; }
}