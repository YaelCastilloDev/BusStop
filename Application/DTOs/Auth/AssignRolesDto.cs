using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class AssignRolesDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
