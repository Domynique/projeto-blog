using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data.Context
{
    public class MeuDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public MeuDbContext(DbContextOptions<MeuDbContext> options) 
            : base(options) 
        { 
        
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }


    }
}
