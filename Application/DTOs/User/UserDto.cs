using Application.DTOs.Route;

namespace Application.DTOs.User
{
    /// <summary>
    /// Represents a user's detailed profile.
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; } = default!;
    }
}