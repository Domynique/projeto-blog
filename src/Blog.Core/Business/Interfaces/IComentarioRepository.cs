using Blog.Core.Business.Models;

namespace Blog.Core.Business.Interfaces
{
    public interface IComentarioRepository : IRepository<Comentario>
    {
        Task<Comentario> ObterComentarioPorPost(Guid id, Guid postId);

        Task<IEnumerable<Comentario>> ObterComentariosPorPost(Guid postId);


    }
}
