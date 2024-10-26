using Blog.Core.Models;

namespace Blog.Core.Interfaces
{
    public interface IComentarioRepository : IRepository<Comentario>
    {
        Task<IEnumerable<Comentario>> ObterComentariosPorPost(Guid postId);

        Task<IEnumerable<Comentario>> ObterComentariosPosts();

        Task<Comentario> ObterComentarioPost(Guid id);

    }
}
