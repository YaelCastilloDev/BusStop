// Application/Services/Interfaces/Authentication/IJwtTokenGenerator.cs
using Domain.Entities;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, string roleName);
}