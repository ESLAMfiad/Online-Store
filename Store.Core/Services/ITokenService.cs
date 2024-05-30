using Microsoft.AspNetCore.Identity;
using Store.Core.Models.identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.Services
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager); //implementation bta3ha hybqa felservice
    }
}
