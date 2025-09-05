using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Queries.Projects;

public class GetProjectByIdQuery : IRequest<ApiResponse<ProjectDto>>
{
    public int Id { get; set; }
}