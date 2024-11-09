using Blog.Core.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Blog.App.Configurations
{
    public static class DbContextConfig
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration) 
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<MeuDbContext>(options =>
                    {
                        options.UseSqlite(connectionString);
                    });

            return services;
        }
    }
}
