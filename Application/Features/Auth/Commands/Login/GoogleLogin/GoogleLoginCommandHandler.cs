using Application.DTOs.Auth;
using Application.Features.Auth.Commands.GoogleLogin; // O el namespace correcto de tu comando
using Application.Features.Auth.Commands.Login.GoogleLogin;
using Application.Services.Interfaces.Authentication;
using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.GoogleLogin
{
    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, AuthResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator; // Asegúrate de tener esta interfaz
        private readonly IRoleRepository _roleRepository;

        public GoogleLoginCommandHandler(
            IUserRepository userRepository,
            IGoogleAuthService googleAuthService,
            IJwtTokenGenerator jwtTokenGenerator,
            IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _googleAuthService = googleAuthService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _roleRepository = roleRepository;
        }

        public async Task<AuthResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Validar Token con Google
            var payload = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);

            // 2. Verificar si el usuario existe usando el REPOSITORIO
            var user = await _userRepository.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                // --- REGISTRAR NUEVO USUARIO ---

                // Usar repositorio para obtener rol
                var userRole = await _roleRepository.GetRoleByNameAsync("User");
                if (userRole == null) throw new Exception("Default role not found.");

                user = new User
                {
                    Email = payload.Email,
                    UserName = payload.Email,
                    GoogleId = payload.Subject,
                    RoleId = userRole.Id,
                    EmailConfirmed = true // Fuente confiable
                };

                // Usar repositorio para crear usuario
                // Nota: CreateAsync generalmente pide password. Para usuarios de Google,
                // puedes generar un password aleatorio fuerte o modificar tu repositorio
                // para aceptar creación sin password si Identity lo permite.
                // Aquí asumo que Identity requiere password, así que generamos uno aleatorio.
                var randomPassword = "Google_" + Guid.NewGuid().ToString("N") + "!";
                var result = await _userRepository.CreateAsync(user, randomPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create Google user");
                }
            }
            else
            {
                // --- USUARIO EXISTE, VINCULAR CUENTA SI ES NECESARIO ---
                if (string.IsNullOrEmpty(user.GoogleId))
                {
                    user.GoogleId = payload.Subject;
                    // Usar repositorio para actualizar
                    await _userRepository.UpdateAsync(user);
                }
            }

            // 3. Generar Nuestro Token JWT
            // Obtenemos el nombre del rol (asumiendo "User" o lógica más compleja)
            // Si necesitamos el rol exacto de la BD:
            // var roles = await _userRepository.GetRolesAsync(user);
            // var roleName = roles.FirstOrDefault() ?? "User";
            var roleName = "User";

            var token = _jwtTokenGenerator.GenerateToken(user, roleName);

            return new AuthResponse
            {
                Token = token,
                Success = true,
                Email = user.Email,
                UserName = user.UserName,
                ISAuthenticated = true,
                Message = "Google Login Successful"
            };
        }
    }
}