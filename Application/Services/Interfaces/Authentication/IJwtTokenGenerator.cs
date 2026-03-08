// Application/Services/Interfaces/Authentication/IJwtTokenGenerator.cs
using Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(Guid userId, string email, string roleName);
}