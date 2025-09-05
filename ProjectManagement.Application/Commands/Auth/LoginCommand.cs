using MediatR;
using ProjectManagement.Shared.DTOs;

namespace ProjectManagement.Application.Commands.Auth;

public class LoginCommand : IRequest<ApiResponse<LoginResponseDto>>
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}