using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Projects;

public class DeleteProjectCommand : IRequest<ApiResponse<bool>>
{
    public int Id { get; set; }
}