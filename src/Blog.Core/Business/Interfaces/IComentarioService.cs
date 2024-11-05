using Blog.Core.Business.Models;

namespace Blog.Core.Business.Interfaces
{
    public interface IComentarioService : IDisposable
    {
        Task<Comentario?> ObterPorId(Guid id);
        Task<List<Comentario>> ObterTodos();
        Task Adicionar(Comentario comentario);
        Task Atualizar(Comentario comentario);
        Task Remover(Guid id);
    }
}
