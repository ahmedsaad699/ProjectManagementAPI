using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Auth;

public class RegisterCommand : IRequest<ApiResponse<string>>
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Role { get; set; }
}