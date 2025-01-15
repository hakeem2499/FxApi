using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(
            50,
            ErrorMessage = "Username must be between 3 and 50 characters.",
            MinimumLength = 3
        )]
        public required string Username { get; init; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public required string Email { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(
            100,
            ErrorMessage = "Password must be between 6 and 100 characters.",
            MinimumLength = 6
        )]
        [DataType(DataType.Password)]
        public required string Password { get; init; }
    }
}
