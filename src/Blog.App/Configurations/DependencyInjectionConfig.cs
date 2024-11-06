﻿using Blog.Core.Business.Interfaces;
using Blog.Core.Data.Repository;
using Blog.Core.Business.Services;
using Blog.Core.Extension;
using Blog.Core.Data.Context;

namespace Blog.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddResolveDependencies(this IServiceCollection services) 
        {
            services.AddScoped<MeuDbContext>();

            services.AddScoped<IAutorRepository, AutorRepository>();
            services.AddScoped<IAutorService, AutorService>();

            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();

            services.AddScoped<IComentarioRepository, ComentarioRepository>();
            services.AddScoped<IComentarioService, ComentarioService>();
           
            services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
            services.AddScoped<IIdentityUserService, IdentityUserService>();
                      
            services.AddScoped<IAppUser, AppUserExtension>();

            return services;
        }
    }
}