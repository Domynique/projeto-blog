using Blog.Api.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Blog.Core.Business.Models;
using Blog.Core.Data.Context;

namespace Blog.Api.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration) 
        {

            services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<MeuDbContext>()
                            .AddRoles<IdentityRole>()
                            .AddDefaultTokenProviders();            

            return services;
        }
    }
}
