using Blog.Core.Notifications;
using Blog.Core.Interfaces;
using Blog.Core.Services;
using Blog.Core.Context;
using Blog.Core.Repository;

namespace Blog.Api.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services) 
        {
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
