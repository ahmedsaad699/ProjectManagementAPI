using AutoMapper;
using MediatR;
using ProjectManagement.Application.Commands.Auth;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Interfaces;
using ProjectManagement.Shared.DTOs;
using BC = BCrypt.Net.BCrypt;

namespace ProjectManagement.Application.Handlers.Auth;

public class RegisterHandler : IRequestHandler<RegisterCommand, ApiResponse<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if username exists
            var existingUser = await _unitOfWork.Repository<User>()
                .SingleOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Username or email already exists"
                };
            }

            var user = _mapper.Map<User>(request);
            user.PasswordHash = BC.HashPassword(request.Password);

            await _unitOfWork.Repository<User>().AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new ApiResponse<string>
            {
                Success = true,
                Message = "User registered successfully",
                Data = "Registration completed"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<string>
            {
                Success = false,
                Message = "An error occurred during registration",
                Errors = new List<string> { ex.Message }
            };
        }
    }
}