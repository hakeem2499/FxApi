using System.ComponentModel.DataAnnotations;

namespace api.Dtos.Account
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public required string Username { get; init; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long.")]
        public required string Password { get; init; }
    }
}
