using Blog.Data.Context;
using Blog.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Blog.Data.Configurations
{
    public static class DbMigrationHelperExtension
    {
        public static void UseDbMigrationHelper(this WebApplication app)
        {
            DbMigrationHelpers.EnsureSeedData(app).Wait();
        }
    }

    public static class DbMigrationHelpers
    {
        public static async Task EnsureSeedData(WebApplication serviceScope)
        {
            var services = serviceScope.Services.CreateScope().ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<MeuDbContext>();

            if (env.IsDevelopment() || env.IsStaging())
            {
                await context.Database.MigrateAsync();

                await EnsureSeedProducts(context);
            }
        }

        private static async Task EnsureSeedProducts(MeuDbContext context)
        {
            if (context.Autores.Any())
            {
                return;
            }

            // Adicionando usuários ao contexto diretamente
            var autor1 = new Autor
            {
                Id = Guid.NewGuid(),
                UserName = "autor123",
                NormalizedUserName = "AUTOR123",
                Email = "autor123@example.com",
                NormalizedEmail = "AUTOR123@TESTE.COM",
                Nome = "Autor Exemplar",
                Biografia = "Autor Exemplar é conhecido por suas contribuições excepcionais na literatura contemporânea.",
                PasswordHash = new PasswordHasher<Autor>().HashPassword(null, "Teste@123"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var autor2 = new Autor
            {
                Id = Guid.NewGuid(),
                UserName = "autor456",
                NormalizedUserName = "AUTOR456",
                Email = "autor456@example.com",
                NormalizedEmail = "AUTOR456@TESTE.COM",
                Nome = "Autor Fictício",
                Biografia = "Autor Fictício é famoso por suas intrigantes narrativas de ficção científica.",
                PasswordHash = new PasswordHasher<Autor>().HashPassword(null, "Teste@123"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await context.Autores.AddRangeAsync(autor1, autor2);
            await context.SaveChangesAsync();

            // Adicionando posts e comentários relacionados
            var post1 = new Post
            {
                Titulo = "Primeiro Post",
                Conteudo = "Este é o conteúdo do primeiro post.",
                AutorId = autor1.Id
            };

            var post2 = new Post
            {
                Titulo = "Segundo Post",
                Conteudo = "Este é o conteúdo do segundo post.",
                AutorId = autor2.Id
            };

            await context.Posts.AddRangeAsync(post1, post2);

            var comentario1 = new Comentario
            {
                Conteudo = "Este é um comentário no primeiro post.",
                PostId = post1.Id,
                AutorId = autor2.Id
            };

            var comentario2 = new Comentario
            {
                Conteudo = "Este é outro comentário no primeiro post.",
                PostId = post1.Id,
                AutorId = autor1.Id
            };

            await context.Comentarios.AddRangeAsync(comentario1, comentario2);

            await context.SaveChangesAsync();
        }
    }
}