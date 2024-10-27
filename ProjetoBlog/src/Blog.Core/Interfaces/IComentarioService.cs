using Blog.Core.Models;

namespace Blog.Core.Interfaces
{
    public interface IComentarioService : IDisposable
    {
        Task<Comentario?> ObterPorId(Guid id);
        Task<List<Comentario>> ObterTodos();
        Task Adicionar(Comentario comentario);
        Task Atualizar(Comentario comentario);
        Task Remover(Guid id);
        Task RemoverComentariosPorPost(Guid id);
    }
}
