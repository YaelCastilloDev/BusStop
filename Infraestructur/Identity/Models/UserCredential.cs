using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Identity.Models
{
    public class UserCredential
    {
        public Guid UsersId { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? PasswordHash { get; set; }
        public string? RefreshToken { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; } //you must add a one to many relationship between UserCredential and RefreshTokens


        public User User { get; set; } = null!; // Propiedad 'User' requerida
    }
}
