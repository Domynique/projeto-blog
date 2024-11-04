using Blog.Core.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Blog.Api.Configurations
{
    public static class DatabaseSelectorExtension
    {
        public static void AddDatabaseSelector(this WebApplicationBuilder builder)
        {

            builder.Services.AddDbContext<MeuDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnectionLite")));

        }
    }
}
