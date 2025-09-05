using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Queries.Tasks;

public class GetTaskByIdQuery : IRequest<ApiResponse<TaskDto>>
{
    public int Id { get; set; }
}