using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class Login
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
