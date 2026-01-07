
using Application.DTOs.Auth;
using Application.Services.Interfaces.Authentication; // <-- Fix for IJwtTokenGenerator
using Application.Services.Interfaces.Repositories;  
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Application.Features.Auth.Commands.Register
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthResponse _jwtTokenGenerator;
        private readonly IRoleRepository _roleRepository; // <-- CHANGE THIS

        // --- Update the constructor ---
        public RegisterUserCommandHandler(
            UserManager<User> userManager,
            IAuthResponse jwtTokenGenerator,
            IRoleRepository roleRepository) // <-- CHANGE THIS
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleRepository = roleRepository; // <-- CHANGE THIS
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null) throw new Exception("Email already in use.");

            // --- Use the repository to get the role ---
            var userRole = await _roleRepository.GetRoleByNameAsync("User");
            if (userRole == null) throw new Exception("Default 'User' role not found.");

            var model = new SignUp
            {
                Email = request.Email,
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(newUser, request.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Registration failed: {errors}");
            }

            // Generate Token
            var token = _jwtTokenGenerator.SignUpAsync(newUser, userRole.Name);

            return new AuthResponse
            {
                Token = token,
                Success = true,
                // ... fill out other AuthResponse properties
            };
        }
    }
}