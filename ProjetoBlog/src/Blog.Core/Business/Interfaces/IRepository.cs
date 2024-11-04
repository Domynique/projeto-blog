using Blog.Core.Business.Models.Base;
using System.Linq.Expressions;

namespace Blog.Core.Business.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<TEntity?> ObterPorId(Guid id);
        Task<List<TEntity>> ObterTodos();
        Task Adicionar(TEntity entity);
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        Task<int> SaveChanges();
    }
}
