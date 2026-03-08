using Application.DTOs.Auth;
using Application.Services.Interfaces.Authentication;
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponseDto>
    {
        // Use IUserRepository to keep the Application layer clean
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IRoleRepository _roleRepository;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleRepository = roleRepository;
        }

        public async Task<AuthResponseDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // 1. Check if email is already in use
            var existingUser = await _userRepository.FindByEmailAsync(request.Email);
            if (existingUser != null) throw new Exception("Email already in use.");

            // 2. Fetch the default role
            // 2. Fetch the default role (Matching your Enum name)
            var userRole = await _roleRepository.GetRoleByNameAsync("BASICUSER");
            if (userRole == null) throw new Exception("Default 'BasicUser' role not found.");

            // 3. Create the Domain Entity
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Username, // Mapping Username from command to Name in Entity
                Email = request.Email,
                EmailVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            // 4. Persist the user via the Repository
            var result = await _userRepository.CreateAsync(newUser, request.Password);

            if (!result.IsSuccess)
            {
                var errors = string.Join(", ", result.Error);
                throw new Exception($"Registration failed: {errors}");
            }

            // 5. Assign the Role (Many-to-Many logic handled by repository)
            await _roleRepository.AssignRoleToUserAsync(newUser.Id, userRole.Id);

            // 6. Generate the JWT Token
            var token = _jwtTokenGenerator.GenerateToken(newUser, userRole.Name);

            return new AuthResponseDto
            {
                Token = token,
                Success = true,
                Email = newUser.Email,
                UserName = newUser.Name,
                ISAuthenticated = true,
                Message = "User registered successfully"
            };
        }
    }
}