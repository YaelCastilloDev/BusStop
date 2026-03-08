using Application.DTOs.Auth;
using Application.Features.Auth.Commands.Login.GoogleLogin;
using Application.Services.Interfaces.Authentication;
using Application.Services.Interfaces.Repositories;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserIdentityRepository _identityRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public GoogleLoginCommandHandler(
            IUserRepository userRepository,
            IUserIdentityRepository identityRepository,
            IRoleRepository roleRepository,
            IGoogleAuthService googleAuthService,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _userRepository = userRepository;
            _identityRepository = identityRepository;
            _roleRepository = roleRepository;
            _googleAuthService = googleAuthService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<AuthResponseDto> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Validate Token with Google
            var payload = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);

            // 2. Check if user exists by Email
            var user = await _userRepository.FindByEmailAsync(payload.Email);

            // Inside Handle method...

            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Name = payload.Name,
                    Email = payload.Email,
                    EmailVerified = true,
                    CreatedAt = DateTime.UtcNow
                };

                // FIX 1: This now uses the passwordless overload we just added
                Result result = await _userRepository.CreateAsyncWithThirdParty(user);
                if (!result.IsSuccess) throw new Exception("Error creating user");

                // FIX 2: Use the correct method name GetRoleByNameAsync
                var defaultRole = await _roleRepository.GetRoleByNameAsync("User");
                if (defaultRole != null)
                {
                    await _roleRepository.AssignRoleToUserAsync(user.Id, defaultRole.Id);
                }

                await _identityRepository.AddIdentityAsync(user.Id, "Google", payload.Subject);
            }
            else
            {
                // --- SCENARIO B: EXISTING USER ---

                // Check if link exists using our new bool method
                bool isLinked = await _identityRepository.ExistsByProviderAsync(user.Id, "Google");

                if (!isLinked)
                {
                    // Link the Google account using primitives
                    await _identityRepository.AddIdentityAsync(user.Id, "Google", payload.Subject);
                }
            }

            // 3. Generate JWT
            var userRoles = await _roleRepository.GetUserRolesAsync(user.Id);
            var roleName = userRoles.FirstOrDefault()?.Name ?? "User";

            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email, roleName);

            return new AuthResponseDto
            {
                Token = token,
                Success = true,
                Email = user.Email,
                UserName = user.Name,
                ISAuthenticated = true,
                Message = "Google Login Successful"
            };
        }
    }
}