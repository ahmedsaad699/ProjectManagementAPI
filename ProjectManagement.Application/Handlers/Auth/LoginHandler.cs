
using MediatR;
using Microsoft.Extensions.Configuration;
using ProjectManagement.Application.Commands.Auth;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Infrastructure.Services;
using ProjectManagement.Shared.DTOs;
using BC = BCrypt.Net.BCrypt;

namespace ProjectManagement.Application.Handlers.Auth;

public class LoginHandler : IRequestHandler<LoginCommand, ApiResponse<LoginResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IConfiguration _configuration;

    public LoginHandler(IUnitOfWork unitOfWork, IJwtService jwtService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _configuration = configuration;
    }

    public async Task<ApiResponse<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.Repository<Domain.Entities.User>()
                .SingleOrDefaultAsync(u => u.Username == request.Username && u.IsActive);

            if (user == null || !BC.Verify(request.Password, user.PasswordHash))
            {
                return new ApiResponse<LoginResponseDto>
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            var token = _jwtService.GenerateToken(user);
            var expirationHours = double.Parse(_configuration["Jwt:ExpirationInHours"]!);

            return new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = new LoginResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role.ToString(),
                    ExpiresAt = DateTime.UtcNow.AddHours(expirationHours)
                }
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<LoginResponseDto>
            {
                Success = false,
                Message = "An error occurred during login",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}