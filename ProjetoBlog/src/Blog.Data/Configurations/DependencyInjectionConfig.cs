using Blog.Data.Models;
using Blog.Data.Notifications;
using Blog.Data.Repository;
using Blog.Data.Services;
using Blog.Data.Validations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Data.Configurations
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
            services.AddScoped<IValidator<Autor>, AutorValidation>();
            services.AddScoped<IValidator<Post>, PostValidation>();
            services.AddScoped<IValidator<Comentario>, ComentarioValidation>();

            return services;
        }
    }
}
