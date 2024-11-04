using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Blog.Core.Business.Interfaces;
using Blog.Core.Business.Models.Base;
using Blog.Core.Data.Context;
using System.Collections.Generic;

namespace Blog.Core.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly MeuDbContext _db;
        protected readonly DbSet<TEntity> _dbSet;
        protected Repository(MeuDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<TEntity>();
            
        }
        public virtual async Task<TEntity?> ObterPorId(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await _dbSet.ToListAsync();
        }
        public virtual async Task Adicionar(TEntity entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            _dbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task Remover(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await SaveChanges();
            }
        }

        public async Task<int> SaveChanges()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
