using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class AssignRoles
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
