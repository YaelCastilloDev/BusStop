using Application.DTOs.Auth;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Identity.Models
{
    public class AppUser:User
    {
        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}
