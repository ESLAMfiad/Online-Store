using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Core.Models.identity;
using System.Security.Claims;

namespace Store.APIs.Extentions
{
    public static class UserManagerExtension
    {
        public static async Task<AppUser?> FindUserWithAddressAsync(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var email= User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u => u.Email == email); //FirstOrDefaultAsync bdl el where linq
            return user; //kda kda msh htrg3 null 3shan lazm ybqa 3aml login w authorized bs el firstordefault mbyfhm4 da lw 4lt el nullable 3ady
        }
    }
}
