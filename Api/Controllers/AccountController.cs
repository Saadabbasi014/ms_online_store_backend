using Api.DTOs;
using Api.Extensions;
using Core.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web; 

namespace Api.Controllers
{
    public class AccountController(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IEmailSender emailSender) : BaseApiController
    {
        // --------- Auth ---------

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto dto)
        {
            var user = new AppUser
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, "Customer");
            return Ok(new { Message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto dto)
        {
            // cookie sign-in (no lockout)
            var result = await signInManager.PasswordSignInAsync(dto.Email, dto.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded) return Unauthorized(new ProblemDetails { Title = "Invalid login attempt" });

            // update last login
            //var user = await userManager.FindByEmailAsync(dto.Email);
            //if (user != null)
            //{
            //    user.LastLoginUtc = DateTime.UtcNow;
            //    await userManager.UpdateAsync(user);
            //}

            return Ok(new { Message = "User logged in successfully" });
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new { Message = "User logged out successfully" });
        }

        [HttpGet("auth-status")]
        public ActionResult GetAuthState()
        {
            return Ok(new { IsAuthenticated = User.Identity?.IsAuthenticated ?? false });
        }

        [HttpGet("user-info")]
        public async Task<ActionResult> GetUserInfo()
        {
            if (User.Identity?.IsAuthenticated == false) return NoContent();
            var user = await signInManager.UserManager.GetUserWithEmailByEmail(User);
            if (user == null) return NoContent();

            var roles = await userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email,
                Roles = roles,
                address = user.Address?.ToDto()
            });
        }

        [Authorize]
        [HttpPost("address")]
        public async Task<ActionResult> CreateOrUpdateAddress(AddressDto addressDto)
        {
            var user = await signInManager.UserManager.GetUserWithEmailByEmail(User);
            if (user == null) return Unauthorized();

            if (user.Address == null)
            {
                user.Address = addressDto.ToEntity();
            }
            else
            {
                user.Address.UpdateFromDto(addressDto);
            }

            var result = await signInManager.UserManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Problem updating user address");
            return Ok(user.Address.ToDto());
        }

        // --------- Roles (Admin-only) ---------

        [Authorize(Roles = "Admin")]
        [HttpPost("roles/create")]
        public async Task<IActionResult> CreateRole(CreateRoleDto dto)
        {
            if (await roleManager.RoleExistsAsync(dto.RoleName))
                return BadRequest("Role already exists");

            var result = await roleManager.CreateAsync(new IdentityRole(dto.RoleName));
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = $"Role '{dto.RoleName}' created." });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            var roles = roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
            return Ok(roles);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("roles/assign")]
        public async Task<IActionResult> AssignRole(AssignRoleDto dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound("User not found");

            if (!await roleManager.RoleExistsAsync(dto.Role))
                return BadRequest("Role does not exist");

            var result = await userManager.AddToRoleAsync(user, dto.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = $"Role '{dto.Role}' assigned to {user.Email}" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("roles/remove")]
        public async Task<IActionResult> RemoveRole(RemoveRoleDto dto)
        {
            var user = await userManager.FindByIdAsync(dto.UserId);
            if (user == null) return NotFound("User not found");

            var result = await userManager.RemoveFromRoleAsync(user, dto.Role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = $"Role '{dto.Role}' removed from {user.Email}" });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("users/{userId}/roles")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found");

            var roles = await userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        // --------- “Active” users (recent logins, e.g., last 15 mins) ---------

        //[Authorize(Roles = "Admin")]
        //[HttpGet("users/active")]
        //public async Task<IActionResult> GetActiveUsers([FromQuery] int withinMinutes = 15)
        //{
        //    var since = DateTime.UtcNow.AddMinutes(-withinMinutes);
        //    var users = await userManager.Users
        //        .Where(u => u.LastLoginUtc != null && u.LastLoginUtc >= since)
        //        .Select(u => new { u.Id, u.Email, u.LastLoginUtc })
        //        .ToListAsync();

        //    return Ok(users);
        //}

        // --------- Forgot/Reset/Change password ---------

        [AllowAnonymous]
        [HttpPost("password/forgot")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return Ok(); // do not reveal if email exists

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encoded = HttpUtility.UrlEncode(token);

            // Build your frontend reset URL (Angular route)
            var resetUrl = $"http://localhost:4200/reset-password?email={HttpUtility.UrlEncode(dto.Email)}&token={encoded}";
            //await emailSender.SendAsync(dto.Email, "Reset your password", $"Click to reset: <a href=\"{resetUrl}\">Reset Password</a>");

            return Ok(new { Message = "If the email exists, a reset link has been sent." });
        }

        [AllowAnonymous]
        [HttpPost("password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null) return BadRequest("Invalid request");

            var result = await userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Password reset successful" });
        }

        [Authorize]
        [HttpPost("password/change")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var result = await userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Password changed successfully" });
        }

        // --------- (Optional) Email confirmation ---------

        [Authorize]
        [HttpPost("email/send-confirmation")]
        public async Task<IActionResult> SendEmailConfirmation()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encoded = HttpUtility.UrlEncode(token);
            var confirmUrl = $"http://localhost:4200/confirm-email?email={HttpUtility.UrlEncode(user.Email)}&token={encoded}";
            //await emailSender.SendAsync(user.Email, "Confirm your email", $"Confirm: <a href=\"{confirmUrl}\">Confirm Email</a>");

            return Ok(new { Message = "Confirmation email sent." });
        }

        [AllowAnonymous]
        [HttpPost("email/confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) return BadRequest("Invalid");

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(new { Message = "Email confirmed." });
        }
    }
}
