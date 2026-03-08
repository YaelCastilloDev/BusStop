// --- Application/Features/Auth/Commands/Login/LoginCommandHandler.cs ---
using Application.Services.Interfaces.Authentication;
using Application.Services.Interfaces.Repositories;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository; // ✨ INYECTAMOS TU REPOSITORIO DE ROLES
        private readonly IJwtTokenGenerator _tokenGenerator;

        public LoginCommandHandler(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IJwtTokenGenerator tokenGenerator)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // 1. Validar Usuario
            var user = await _userRepository.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new Exception("Credenciales inválidas.");
            }

            // 2. Validar Contraseña
            var isPasswordValid = await _userRepository.CheckPasswordAsync(user, request.Password);
            if (!isPasswordValid)
            {
                throw new Exception("Credenciales inválidas.");
            }

            // 3. ✨ OBTENER ROLES USANDO EL ROLE REPOSITORY ✨
            var userRoles = await _roleRepository.GetUserRolesAsync(user.Id);

            // Extraemos el nombre del primer rol, o "User" si la lista está vacía
            var roleName = userRoles.FirstOrDefault()?.Name ?? "User";

            // 4. Generar Token
            return _tokenGenerator.GenerateToken(user.Id, user.Email!, roleName);
        }
    }
}