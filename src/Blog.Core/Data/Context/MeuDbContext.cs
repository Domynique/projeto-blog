using Blog.Core.Business.Models;
using Blog.Core.Business.Models.Base;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Blog.Core.Data.Context
{
    public class MeuDbContext : IdentityDbContext
    {
        public MeuDbContext(DbContextOptions<MeuDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                                                       .SelectMany(e => e.GetProperties()
                                                       .Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            foreach (var softDeleteEntity in typeof(Entity).Assembly.GetTypes()
                                                                    .Where(type => typeof(Entity)
                                                                    .IsAssignableFrom(type) 
                                                                    && type.IsClass 
                                                                    && !type.IsAbstract))
            {
                var parameter = Expression.Parameter(softDeleteEntity, "x");
                var ativoProperty = Expression.Property(parameter, "Ativo");
                var ativoCheck = Expression.Equal(ativoProperty, Expression.Constant(true));

                var lambda = Expression.Lambda(ativoCheck, parameter);
                modelBuilder.Entity(softDeleteEntity).HasQueryFilter(lambda);
            }
                /*
                foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                                                               .SelectMany(e => e.GetForeignKeys()))
                {
                    relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
                }
                */
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);
                
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries()
                                               .Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null || 
                                                               entry.Entity.GetType().GetProperty("DataAlteracao") != null || 
                                                               entry.Entity.GetType().GetProperty("Ativo") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                    entry.Property("Ativo").CurrentValue = true;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                    entry.Property("DataAtualizacao").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Deleted)
                {
                    entry.Property("DataCadastro").IsModified = false;
                    entry.Property("DataAtualizacao").CurrentValue = DateTime.Now;
                    entry.Property("Ativo").CurrentValue = false;
                   
                    entry.State = EntityState.Modified;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
