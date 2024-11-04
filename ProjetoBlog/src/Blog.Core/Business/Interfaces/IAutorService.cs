using Blog.Core.Business.Models;

namespace Blog.Core.Business.Interfaces
{
    public interface IAutorService : IDisposable
    {
        Task<Autor?> ObterPorId(Guid id);
        Task<List<Autor>> ObterTodos();

    }
}
