using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Api.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;
        public bool IsPersistent { get; set; } 
    }
}
