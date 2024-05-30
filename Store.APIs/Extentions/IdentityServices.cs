using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Models.identity;
using Store.Core.Services;
using Store.Repository.identity;
using Store.Service;
using System.Text;

namespace Store.APIs.Extentions
{
    public static class IdentityServices
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection Services,IConfiguration configuration)
        {
           Services.AddScoped<ITokenService, TokenService>();
           Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();           //de wel t7tha ll identity
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme; //dol bdl maktbha fel controller 3ndl authorize
            }).AddJwtBearer(options=>
            {
                options.TokenValidationParameters = new TokenValidationParameters() { 
                    ValidateIssuer= true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            };
            }); //user,sigin,role managers
            // faydt elschema l foq Zyadt aman 3shan a3rf n eltoken t5osny
           return Services;

        }
    }
}
