using Blog.Business.Models;

namespace Blog.Business.Interfaces
{
    public interface IComentarioRepository : IRepository<Comentario>
    {
        Task<IEnumerable<Comentario>> ObterComentariosPorPost(Guid postId);

        Task<IEnumerable<Comentario>> ObterComentariosPosts();

        Task<Comentario> ObterComentarioPost(Guid id);

    }
}
