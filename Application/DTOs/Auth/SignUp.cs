using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class SignUp
    {
        // Corresponds to the single 'Name' field in your Domain.User.
        // It's the full name the user provides.
        [Required]
        [StringLength(100, ErrorMessage = "The Name must be between {2} and {1} characters.", MinimumLength = 3)]
        public string Name { get; set; }

        // Corresponds to 'email' VARCHAR(45) in your database (User entity).
        // Using a practical limit (e.g., 128) is safer than 45 for emails.
        [Required]
        [EmailAddress]
        [StringLength(128, ErrorMessage = "Email cannot exceed 128 characters.")]
        public string Email { get; set; }

        // Standard field for security. This will be hashed before storage.
        [Required]
        [StringLength(256, ErrorMessage = "Password cannot exceed 256 characters.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // OPTIONAL: If you want the user to confirm the password.
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
