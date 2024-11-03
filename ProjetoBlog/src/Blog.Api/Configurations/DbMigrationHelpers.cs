using Blog.Core.Context;
using Blog.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            
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

        private static async Task EnsureSeedProducts(MeuDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (context.Autores.Any())
            {
                return;
            }

            var autor1 = new Autor
            {
                Id = Guid.NewGuid(),
                Nome = "Hugo Nunes",
                Biografia = "Hugo Nunes é conhecido por suas contribuições excepcionais na literatura contemporânea.",
            };

            var autor2 = new Autor
            {
                Id = Guid.NewGuid(),
                Nome = "Paula Nunes",
                Biografia = "Paula Nunes é famosa por suas intrigantes narrativas de ficção científica.",
            };

            await context.Autores.AddRangeAsync(autor1, autor2);
            await context.SaveChangesAsync(); // Certifique-se de salvar antes de usar os IDs

            var applicationUser1 = new ApplicationUser
            {
                UserName = "hugonunes@teste.com",
                NormalizedUserName = "HUGONUNES@TESTE.COM",
                Email = "hugonunes@teste.com",
                NormalizedEmail = "HUGONUNES@TESTE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                AutorId = autor1.Id
            };

            var applicationUser2 = new ApplicationUser
            {
                UserName = "paulanunes@teste.com",
                NormalizedUserName = "PAULANUNES@TESTE.COM",
                Email = "paulanunes@teste.com",
                NormalizedEmail = "PAULANUNES@TESTE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                AutorId = autor2.Id
            };

            applicationUser1.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(applicationUser1, "Teste@123");
            applicationUser2.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(applicationUser2, "Teste@123");

            await userManager.CreateAsync(applicationUser1);
            await userManager.CreateAsync(applicationUser2);

            await userManager.AddClaimAsync(applicationUser1, new Claim("AutorId", applicationUser1.AutorId.ToString())); 
            await userManager.AddClaimAsync(applicationUser2, new Claim("AutorId", applicationUser2.AutorId.ToString()));


            if (!await roleManager.RoleExistsAsync("Admin")) 
            { 
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin")); 
            }

            await userManager.AddToRoleAsync(applicationUser1, "Admin");

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

            await context.SaveChangesAsync(); // Salva todas as alterações uma vez no final
        }


        private static async Task EnsureRoleExists(RoleManager<IdentityRole<Guid>> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }

    }
}