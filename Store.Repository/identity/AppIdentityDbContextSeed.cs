using Microsoft.AspNetCore.Identity;
using Store.Core.Models.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository.identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "eslam hossny",
                    Email = "eslam.hossny2001@gmail.com",
                    UserName = "lamXes",
                    PhoneNumber = "01066014117"
                };
                await userManager.CreateAsync(user, "password");
            }
        }
    }
}
