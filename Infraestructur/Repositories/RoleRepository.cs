using Application.Services.Interfaces.Repositories;
using Domain.Entities;
using Infraestructur.Data;
using Infraestructur.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infraestructur.Repositories
{
    public class RoleRepository : IRoleRepository
    {
private readonly RoleManager<AppRole> _roleManager;
    private readonly UserManager<UserCredential> _userManager;

    public RoleRepository(RoleManager<AppRole> roleManager, UserManager<UserCredential> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task AssignRoleToUserAsync(Guid userId, Guid roleId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var appRole = await _roleManager.FindByIdAsync(roleId.ToString());

        if (user != null && appRole != null)
        {
            await _userManager.AddToRoleAsync(user, appRole.Name!);
        }
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        var appRole = await _roleManager.FindByNameAsync(roleName);
        if (appRole == null) return null;

        return new Role { Id = appRole.Id, Name = appRole.Name! };
    }

        public async Task<IEnumerable<Role>> GetUserRolesAsync(Guid userId)
        {
            // 1. Buscamos la credencial (identidad) del usuario
            var userCredential = await _userManager.FindByIdAsync(userId.ToString());

            if (userCredential == null) return Enumerable.Empty<Role>();

            // 2. Obtenemos los nombres de los roles asociados
            var roleNames = await _userManager.GetRolesAsync(userCredential);

            // 3. Mapeamos a nuestras entidades de Dominio
            // Nota: Aquí podrías querer traer los IDs reales de la DB si los necesitas
            var roles = new List<Role>();
            foreach (var name in roleNames)
            {
                var appRole = await _roleManager.FindByNameAsync(name);
                if (appRole != null)
                {
                    roles.Add(new Role
                    {
                        Id = appRole.Id,
                        Name = appRole.Name!
                    });
                }
            }
            return roles;
        }
    }
}