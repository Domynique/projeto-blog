using Blog.Core.Models;

namespace Blog.Core.Interfaces
{
    public interface IPostService : IDisposable
    {
        Task<Post?> ObterPorId(Guid id);
        Task<List<Post>> ObterTodos();
        Task Adicionar(Post post);
        Task Atualizar(Post post);
        Task Remover(Guid id, string userId, bool isAdmin);
    }
}
