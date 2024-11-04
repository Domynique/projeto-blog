using Blog.Core.Business.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Data.Context
{
    public class MeuDbContext : IdentityDbContext
    {
        public MeuDbContext(DbContextOptions<MeuDbContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
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

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                                                           .SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

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
