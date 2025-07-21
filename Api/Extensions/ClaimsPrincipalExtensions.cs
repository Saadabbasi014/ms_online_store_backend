using Core.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using System.Security.Claims;

namespace Api.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static async Task<AppUser?> GetUserByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var returnUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == user.GetUserEmail());
            if (returnUser == null) throw new AuthenticationException("User not found with the provided email.");
            return returnUser;
        }

        public static async Task<AppUser?> GetUserWithEmailByEmail(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users
                .Include(x => x.Address)
                .FirstOrDefaultAsync(u => u.Email == user.GetUserEmail());
        }

        public static string GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        }
    }
} 
