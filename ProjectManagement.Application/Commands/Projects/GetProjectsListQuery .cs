using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Projects;

public class GetProjectsListQuery : IRequest<ApiResponse<PagedResultDto<ProjectDto>>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
    public string? Search { get; set; }
    public int? Status { get; set; }
    public int? ManagerId { get; set; }
}
