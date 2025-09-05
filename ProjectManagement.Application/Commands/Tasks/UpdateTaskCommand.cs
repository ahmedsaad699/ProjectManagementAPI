using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Tasks;

public class UpdateTaskCommand : IRequest<ApiResponse<TaskDto>>
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? AssignedToId { get; set; }
    public int Priority { get; set; }
    public DateTime DueDate { get; set; }
    public int Status { get; set; }
}