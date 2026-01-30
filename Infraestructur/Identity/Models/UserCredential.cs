using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Infraestructur.Identity.Models
{
    // This represents the 'user_credentials' table
    public class UserCredential : IdentityUser<Guid>
    {
        // IdentityUser already has PasswordHash, NormalizedEmail, and Id.
        // We map 'Id' to your 'users_id' column in the DbContext.

        public string? RefreshToken { get; set; } //shouldnt use it 
        public List<RefreshToken> RefreshTokens = new List<RefreshToken>();

        // Navigation property to the Domain User
        public virtual User User { get; set; } = null!;
    }
}