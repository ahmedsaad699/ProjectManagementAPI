using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Projects;

public class UpdateProjectCommand : IRequest<ApiResponse<ProjectDto>>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }
    public decimal Budget { get; set; }
}