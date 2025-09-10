namespace Api.DTOs
{
    public class CreateRoleDto { public required string RoleName { get; set; } }

    public class AssignRoleDto
    {
        public required string UserId { get; set; }
        public required string Role { get; set; }
    }

    public class RemoveRoleDto
    {
        public required string UserId { get; set; }
        public required string Role { get; set; }
    }

    public class ForgotPasswordDto { public required string Email { get; set; } }

    public class ResetPasswordDto
    {
        public required string Email { get; set; }
        public required string Token { get; set; }  // URL-decoded token
        public required string NewPassword { get; set; }
    }

    public class ChangePasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}

