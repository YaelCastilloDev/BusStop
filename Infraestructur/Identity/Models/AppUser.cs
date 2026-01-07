using Application.DTOs.Auth;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructur.Identity.Models
{
    public class AppUser: IdentityUser<Guid>

    {

        public string? GoogleId { get; set; }

        public int RoleId { get; set; }
        public AppRole Role { get; set; } = default!;

        public ICollection<Route> CreatedRoutes { get; set; } = new List<Route>();
        public ICollection<Route> DeletedRoutes { get; set; } = new List<Route>();
        public ICollection<Stop> CreatedStops { get; set; } = new List<Stop>();
        public ICollection<Stop> DeletedStops { get; set; } = new List<Stop>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<CommentReaction> CommentReactions { get; set; } = new List<CommentReaction>();

        public List<RefreshToken>? RefreshTokens { get; set; }


    }
}
