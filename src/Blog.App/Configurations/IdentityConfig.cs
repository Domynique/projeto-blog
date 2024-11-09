using Microsoft.AspNetCore.Identity;
using Blog.Core.Data.Context;

namespace Blog.App.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services) 
        {
            services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<MeuDbContext>();

            return services;
        }
    }
}
