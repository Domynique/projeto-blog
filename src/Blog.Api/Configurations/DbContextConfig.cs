using Blog.Core.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Api.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) 
        {
            services.AddDbContext<MeuDbContext>(options =>
                    {
                        options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
                    });

            return services;
        }
    }
}
