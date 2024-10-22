using Blog.Data.Context;
using Blog.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Configurations
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
            var userManager = serviceProvider.GetRequiredService<UserManager<Autor>>();
            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var context = scope.ServiceProvider.GetRequiredService<MeuDbContext>();

            if (env.IsDevelopment() || env.IsStaging())
            {
                await context.Database.MigrateAsync();

                await EnsureSeedProducts(context, userManager, roleManager);
            }
        }

        private static async Task EnsureSeedProducts(MeuDbContext context, UserManager<Autor> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {           

            if (context.Autores.Any())
            {
                return;
            }

            var autor1 = new Autor
            {
                Id = Guid.NewGuid(),
                UserName = "hugonunes@example.com",
                NormalizedUserName = "HUGONUNES@EXAMPLE.COM",
                Email = "hugonunes@example.com",
                NormalizedEmail = "HUGONUNES@EXAMPLE.COM",
                Nome = "Hugo Nunes",
                Biografia = "Hugo Nunes é conhecido por suas contribuições excepcionais na literatura contemporânea.",
                PasswordHash = new PasswordHasher<Autor>().HashPassword(null, "Teste@123"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var autor2 = new Autor
            {
                Id = Guid.NewGuid(),
                UserName = "paulanunes@example.com",
                NormalizedUserName = "PAULANUNES@EXAMPLE.COM",
                Email = "paulanunes@example.com",
                NormalizedEmail = "PAULANUNES@EXAMPLE.COM",
                Nome = "Paula Nunes",
                Biografia = "Paula Nunes é famosa por suas intrigantes narrativas de ficção científica.",
                PasswordHash = new PasswordHasher<Autor>().HashPassword(null, "Teste@123"),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await context.Autores.AddRangeAsync(autor1, autor2);

            await context.SaveChangesAsync();

            // Verificar se a role "Admin" existe, e cria se necessário
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            // Adicionar role "Admin" ao primeiro autor
            await userManager.AddToRoleAsync(autor1, "Admin"); 


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