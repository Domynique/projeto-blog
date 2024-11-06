using Microsoft.AspNetCore.Identity;
using Blog.Core.Data.Context;

namespace Blog.App.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services) 
        {

            services.AddIdentity<IdentityUser, IdentityRole>()
                            .AddEntityFrameworkStores<MeuDbContext>()
                            .AddRoles<IdentityRole>()
                            .AddDefaultTokenProviders();            

            return services;
        }
    }
}
