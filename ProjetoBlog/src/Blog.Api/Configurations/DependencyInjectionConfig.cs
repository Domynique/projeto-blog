using Blog.Business.Notifications;
using Blog.Business.Interfaces;
using Blog.Business.Services;
using Blog.Data.Context;
using Blog.Data.Repository;

namespace Blog.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services) 
        {
            services.AddScoped<MeuDbContext>();
            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<IAutorService, AutorService>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IComentarioRepository, ComentarioRepository>();
            services.AddScoped<IComentarioService, ComentarioService>();
            services.AddScoped<INotificador, Notificador>();

            return services;
        }
    }
}
