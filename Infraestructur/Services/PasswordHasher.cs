using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infraestructur.Services
{
    // Update the generic type to User
    public class PasswordHasher : IPasswordHasher<User>
    {
        // Interface implementation: HashPassword
        public string HashPassword(User user, string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Interface implementation: VerifyHashedPassword
        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            bool isValid = BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);

            return isValid
                ? PasswordVerificationResult.Success
                : PasswordVerificationResult.Failed;
        }
    }
}