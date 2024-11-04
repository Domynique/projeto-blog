using Blog.Core.Business.Models;
using Blog.Core.Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var autor1 = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "hugonunes@teste.com",
                NormalizedUserName = "HUGONUNES@TESTE.COM",
                Email = "hugonunes@teste.com",
                NormalizedEmail = "HUGONUNES@TESTE.COM",
                EmailConfirmed = true,
                
            };

            var autor2 = new IdentityUser
            {
                UserName = "paulanunes@teste.com",
                NormalizedUserName = "PAULANUNES@TESTE.COM",
                Email = "paulanunes@teste.com",
                NormalizedEmail = "PAULANUNES@TESTE.COM",
                EmailConfirmed = true,

            };

            autor1.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(autor1, "Teste@123");
            autor2.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(autor2, "Teste@123");

            var post1 = new Post
            {
                Titulo = "Primeiro Post",
                Conteudo = "Este é o conteúdo do primeiro post.",
                Autor = new Autor
                {
                    UserId = autor1.Id,
                    DataCadastro = DateTime.Now,
                    DataAtualizacao = DateTime.Now,
                    Ativo = true
                },
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now,
                Comentarios = new List<Comentario>
                {
                    new()
                    {
                        Conteudo = "Este é um comentário no primeiro post.",
                        DataCadastro= DateTime.Now,
                        DataAtualizacao= DateTime.Now,
                        UserId = autor2.Id
                    }
                }
                
            };

            var post2 = new Post
            {
                Titulo = "Segundo Post",
                Conteudo = "Este é o conteúdo do segundo post.",
                Autor = new Autor
                {
                    UserId = autor2.Id,
                    DataCadastro = DateTime.Now,
                    DataAtualizacao = DateTime.Now,
                    Ativo = true
                },
                DataCadastro = DateTime.Now,
                DataAtualizacao = DateTime.Now,
                Comentarios = new List<Comentario>
                {
                    new()
                    {
                        Conteudo = "Este é um comentário no segundo post.",
                        DataCadastro= DateTime.Now,
                        DataAtualizacao= DateTime.Now,
                        UserId = autor1.Id
                    },
                    new()
                    {
                        Conteudo = "Este é um outro comentário no segundo post.",
                        DataCadastro= DateTime.Now,
                        DataAtualizacao= DateTime.Now,
                        UserId = autor2.Id
                    }
                }

            };


            await context.Users.AddAsync(autor1);
            await context.Users.AddAsync(autor2);
            await context.Posts.AddRangeAsync(post1, post2);
            await context.SaveChangesAsync();
        }


    }
}