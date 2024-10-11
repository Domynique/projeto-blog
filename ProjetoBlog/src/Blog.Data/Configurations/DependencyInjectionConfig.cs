using Microsoft.Extensions.DependencyInjection;

namespace Blog.Data.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection service) 
        {

            service.AddScoped<IAutorRepository, AutorRepository>();
            service.AddScoped<IAutorService, AutorService>();
            service.AddScoped<IComentarioRepository, ComentarioRepository>();
            service.AddScoped<IComentarioService, ComentarioService>();

            return service;
        }
    }
}
